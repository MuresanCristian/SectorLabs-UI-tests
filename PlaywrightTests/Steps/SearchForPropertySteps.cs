using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightTests.Core;
using PlaywrightTests.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using static PlaywrightTests.Helpers.Utils;

namespace PlaywrightTests.Steps
{
    [Binding]
    public class SearchForPropertySteps : PageTest
    {
        public readonly ScenarioContext _scenarioContext;

        private CommonPageObjects _common;
        private IPage _page;

        public SearchForPropertySteps(IPage page, CommonPageObjects common, ScenarioContext scenarioContext)
        {
            _page = page;
            _common = common;
            _scenarioContext = scenarioContext;
        }

        [Given(@"User navigates to (.*)")]
        public async Task GivenUserNavigatesToAirBnb(string websiteUrl)
        {
            await _common.NavigateAsync(AppSettingsParamReplace(AppSettings.BaseUrl, websiteUrl));
        }

        [When(@"User search for property using data")]
        public async Task WhenUserSearchForPropertyUsingData(Table searchByDetails)
        {
            //Translate into dictionary the table that comes from gherkin steps
            var dictionary = TableExtensions.ToDictionary(searchByDetails);
            _scenarioContext["tableAsDictionary"] = searchByDetails;

            //Action to expand the filters section
            await _page.WaitForSelectorAsync(AppSettings.Locators.ExpandFilters);
            await _page.Locator(AppSettings.Locators.ExpandFilters).ClickAsync();

            //Action to select check in date
            var checkInDate = DateTime.Now.AddDays(AddDaysProcessor(dictionary["Check in"])).ToString("MM/dd/yyyy");
            await _page.Locator(GetLocatorByDateFormat(checkInDate)).ClickAsync();
            _scenarioContext["checkInDate"] = checkInDate;

            //Action to select check out date
            var checkOutDate = DateTime.Parse(checkInDate).AddDays(AddDaysProcessor(dictionary["Check out"])).ToString("MM/dd/yyyy");
            await _page.Locator(GetLocatorByDateFormat(checkOutDate)).ClickAsync();
            _scenarioContext["checkOutDate"] = checkOutDate;

            //Action to select location
            await _page.Locator(GetLocatorByLabelName(searchByDetails.Rows[0].Values.First())).FillAsync(dictionary["Where"]);

            //Action to select guests
            await _page.Locator(AppSettings.Locators.ExpandGuestsFilter).ClickAsync();
            var guestsList = GuestsProcessor(dictionary["Who"]);
            foreach (string guest in guestsList)
            {
                await _page.Locator(GetLocatorByGuestType(guest)).ClickAsync();
            }

            //Action to perform search by given filters
            await _page.Locator(AppSettings.Locators.SearchButton).ClickAsync();

        }

        [When("User enters extra filters")]
        public async Task WhenUserEntersExtraFilters(Table searchByExtraDetails)
        {
            //Translate into dictionary the table that comes from gherkin steps
            var dictionary = TableExtensions.ToDictionary(searchByExtraDetails);
            _scenarioContext["extraDetailsAsDictionary"] = searchByExtraDetails;

            await Task.Delay(1000);

            //Action to expand the extra filters section
            await _page.WaitForSelectorAsync(AppSettings.Locators.ExpandExtraFilters);
            await _page.Locator(AppSettings.Locators.ExpandExtraFilters).ClickAsync();

            //Action to select number of bedrooms
            await _page.WaitForSelectorAsync(AppSettingsParamReplace(AppSettings.Locators.FilterNumberOfBedrooms, dictionary["Bedrooms"]));
            await _page.Locator(AppSettingsParamReplace(AppSettings.Locators.FilterNumberOfBedrooms, dictionary["Bedrooms"])).ClickAsync();

            //Action to select extra facilities
            await _page.Locator(AppSettings.Locators.AmenititesShowMore).ClickAsync();
            await _page.Locator(AppSettingsParamReplace(AppSettings.Locators.AmenititesSelection, dictionary["Facilities"])).ClickAsync();

            //Action to perform search by given extra filters
            await _page.Locator(AppSettings.Locators.ApplyExtraFiltersButton).ClickAsync();
        }

        [When("User hover over first property from the list")]
        public async Task WhenUserHoverOverFirstPropertyFromTheList()
        {
            await _page.WaitForLoadStateAsync();

            //Iterate thourg all pins to get background color
            var allPinsFromMap = await _page.QuerySelectorAllAsync(AppSettingsParamReplace(AppSettings.Locators.GetAllPinsBackgroundColor, "*"));
            List<string> pinBgColorBeforeHover = new List<string>();
            for (int i = 1; i <= allPinsFromMap.Count; i++)
            {
                pinBgColorBeforeHover.Add(GetBackgroundColorFromCssStyle(_page.Locator(AppSettingsParamReplace(AppSettings.Locators.GetAllPinsBackgroundColor, i.ToString())).GetAttributeAsync("style").Result));
            }

            //Hover over the first property from the page
            await _page.Locator(AppSettingsParamReplace(AppSettings.Locators.IterateEachFirstPageProperty, "1")).HoverAsync();

            //Check that property is displayed on the map by waiting for selector that dinamically trigger when
            //a property is hovered
            await _page.WaitForSelectorAsync(AppSettings.Locators.PropertyMapPinDinamicSelection);

            //Get the background color after hover and iterate thourgh initial list to check that color has changed
            var bgColorAfterHover = GetBackgroundColorFromCssStyle(_page.Locator(AppSettings.Locators.PropertyMapPinDinamicBackground).GetAttributeAsync("style").Result);
            foreach (string color in pinBgColorBeforeHover)
            {
                Assert.That(bgColorAfterHover, Is.Not.EqualTo(color));
            }
        }

        [When("User click on property on the map")]
        public async Task WhenUserClickOnPropertyOnTheMap()
        {
            await _page.Locator(AppSettings.Locators.PropertyMapPinDinamicSelection).ClickAsync();
        }

        [Then("Verify that filters are applied correctly")]
        public async Task ThenVerifyThatFiltersAreAppliedCorrectly()
        {
            //Get data from When step trough Scenario Context
            var inputTable = _scenarioContext["tableAsDictionary"];
            var dictionary = TableExtensions.ToDictionary((Table)inputTable);

            //Arrange data for Location assertion
            var cityLocation = dictionary["Where"].Split(",").First();
            var filterLocationLocator = _page.Locator(GetLocatorByFilterArea("Location"));
            await Expect(filterLocationLocator).ToHaveTextAsync(cityLocation);

            //Arrange data for Check in / Check out assertion
            //ConstructDateStringFormat() returns output as "MMM DD - DD" format if both dates are from same month
            //or return "MMM DD - MMM DD" if checkin month differ from checkout month
            var checkIn = DateTime.Parse(_scenarioContext["checkInDate"].ToString());
            var checkOut = DateTime.Parse(_scenarioContext["checkOutDate"].ToString());
            var checkInOutDate = ConstructDateStringFormat(checkIn, checkOut);
            var filterCheckInOutLocator = _page.Locator(GetLocatorByFilterArea("Check in / Check out"));
            await Expect(filterCheckInOutLocator).ToHaveTextAsync(checkInOutDate);

            //Arrange data for Guests assertion
            var numberOfGuests = GuestsProcessor(dictionary["Who"]).Count;
            var filterGuestsLocator = _page.Locator(GetLocatorByFilterArea("Guests"));
            await Expect(filterGuestsLocator).ToHaveTextAsync(numberOfGuests + " guests");
        }

        [Then("Verify properties match the guests request")]
        public async Task ThenVerifyPropertiesMatchTheGuestsRequest()
        {
            //Get data from When step trough Scenario Context
            var inputTable = _scenarioContext["tableAsDictionary"];
            var dictionary = TableExtensions.ToDictionary((Table)inputTable);

            //The airBnb dashboard doest not display the number of guests that property accepts so we have to iterate throught
            //all properties in order to verify that
            var numberOfPropertiesInFirstPage = await _page.QuerySelectorAllAsync(AppSettings.Locators.NumberOfPropertiesInFirstPage);

            //Loop between all properties to check if field 'guests' is greater or equals than number of guests inserted in filter
            for (int i = 1; i <= numberOfPropertiesInFirstPage.Count; i++)
            {
                var _newPage = await _page.Context.RunAndWaitForPageAsync(async () =>
                {
                    await _page.Locator(AppSettingsParamReplace(AppSettings.Locators.IterateEachFirstPageProperty, i.ToString())).ClickAsync();
                });

                var propertyAllowedGuestsText = await _newPage.Locator(AppSettings.Locators.PropertyAllowedGuests).InnerTextAsync();
                var propertyAllowedGuestsNumber = Int32.Parse(propertyAllowedGuestsText.Split(' ').First());

                Assert.That(propertyAllowedGuestsNumber, Is.GreaterThanOrEqualTo(GuestsProcessor(dictionary["Who"]).Count));
                await _newPage.CloseAsync();
            }
        }

        [Then("Verify that extra filters are applied correctly")]
        public async Task ThenVerifyThatExtraFiltersAreAppliedCorrectly()
        {
            //Get data from When step trough Scenario Context
            var inputTable = _scenarioContext["extraDetailsAsDictionary"];
            var dictionary = TableExtensions.ToDictionary((Table)inputTable);
            await _page.WaitForLoadStateAsync();

            //The airBnb dashboard doest not display the number of guests that property accepts so we have to iterate throught
            //all properties in order to verify that
            var numberOfPropertiesInFirstPage = await _page.QuerySelectorAllAsync(AppSettings.Locators.NumberOfPropertiesInFirstPage);

            //Loop between all properties to check if field 'bedrooms' is greater or equals than number of bedrooms inserted in filter
            for (int i = 1; i <= numberOfPropertiesInFirstPage.Count; i++)
            {
                var propertyNumberOfBedroomsText = await _page.Locator(AppSettingsParamReplace(AppSettings.Locators.NumberOfBedroomsInDashboard, i.ToString())).InnerTextAsync();
                var propertyNumberOfBedroomsNumber = Int32.Parse(propertyNumberOfBedroomsText.Split(' ').First());

                Assert.That(propertyNumberOfBedroomsNumber, Is.GreaterThanOrEqualTo(Int32.Parse(dictionary["Bedrooms"])));
            }
        }

        [Then("Verify first property details match the extra options")]
        public async Task ThenVerifyFirstPropertyDetailsMatchTheExtraOptions()
        {
            //Get data from When step trough Scenario Context
            var inputTable = _scenarioContext["extraDetailsAsDictionary"];
            var dictionary = TableExtensions.ToDictionary((Table)inputTable);

            //Click on the first property from the page
            var _newPage = await _page.Context.RunAndWaitForPageAsync(async () =>
            {
                await _page.Locator(AppSettingsParamReplace(AppSettings.Locators.IterateEachFirstPageProperty, "1")).ClickAsync();
            });

            //Handles translation modal so that website stays in EN
            await _newPage.Locator(AppSettings.Locators.TranslationButtonModal).ClickAsync();
            await _newPage.Locator(AppSettings.Locators.TranslationToggle).ClickAsync();

            //Expand all amenitites modal
            await _newPage.Locator(AppSettings.Locators.OpenAllPropertyAmenities).ClickAsync();

            await Task.Delay(1000);

            //Get all facilities and iterate through them to verify if match the pool word
            var numberOfFacilitites = await _newPage.QuerySelectorAllAsync(AppSettingsParamReplace(AppSettings.Locators.AllFacilitiesAmenitites, "*"));

            bool flag = false;
            for (int i = 1; i <= numberOfFacilitites.Count; i++)
            {
                var facility = await _newPage.Locator(AppSettingsParamReplace(AppSettings.Locators.AllFacilitiesAmenitites, (i + 1).ToString())).InnerTextAsync();
                if (facility.Contains(dictionary["Facilities"], StringComparison.OrdinalIgnoreCase))
                {
                    flag = true;
                }
            }

            Assert.That(flag, Is.True);
            await _newPage.CloseAsync();
        }

        [Then("Verify that details from map popup and property list match")]
        public async Task ThenVerifyThatDetailsFromMapPopupAndPropertListMatch()
        {
            //Get data from map modal
            var mapCardTitle = await _page.Locator(AppSettings.Locators.PropertyTitleMapCard).InnerTextAsync();
            var mapCardDescription = await _page.Locator(AppSettings.Locators.PropertyDescriptionMapCard).InnerTextAsync();

            //Get data from dashboard property card
            var firstPropertyTitle = await _page.Locator(AppSettings.Locators.FirstPropertyTitle).InnerTextAsync();
            var firstPropertyDescription = await _page.Locator(AppSettings.Locators.FirstPropertyDescription).InnerTextAsync();

            Assert.Multiple(() =>
            {
                Assert.That(mapCardTitle, Is.EqualTo(firstPropertyTitle));
                Assert.That(mapCardDescription, Is.EqualTo(firstPropertyDescription));
            });
        }
    }
}