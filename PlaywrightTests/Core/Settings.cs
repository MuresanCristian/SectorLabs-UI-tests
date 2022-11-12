using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace PlaywrightTests.Core
{
    public sealed class Settings
    {
        private static readonly Lazy<Settings> lazy = new Lazy<Settings>(() => new Settings());

        public static Settings Instance { get { return lazy.Value; } }

        public CustomSettings CustomSettings { get; set; }

        private Settings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            CustomSettings = configuration.Get<CustomSettings>();
        }
    }

    public class CustomSettings
    {
        public App App { get; set; }
        public Locators Locators { get; set; }
    }

    public class App
    {
        public string BaseUrl { get; set; }
    }

    public class Locators
    {
        public Filters Filters { get; set; }
    }

    public class Filters
    {
        public string ExpandFilters { get; set; }
        public string ExpandExtraFilters { get; set; }
        public string FillInputUnderLabel { get; set; }
        public string SearchButton { get; set; }
        public string AddGuestsBasedOnGuestType { get; set; }
        public string CalendarDaySelector { get; set; }
        public string ExpandGuestsFilter { get; set; }
        public string FilterValues { get; set; }
        public string NumberOfPropertiesInFirstPage { get; set; }
        public string IterateEachFirstPageProperty { get; set; }
        public string PropertyAllowedGuests { get; set; }
        public string FilterNumberOfBedrooms { get; set; }
        public string AmenititesShowMore { get; set; }
        public string AmenititesSelection { get; set; }
        public string ApplyExtraFiltersButton { get; set; }
        public string NumberOfBedroomsInDashboard { get; set; }
        public string OpenAllPropertyAmenities { get; set; }
        public string AllFacilitiesAmenitites { get; set; }
        public string TranslationButtonModal { get; set; }
        public string TranslationToggle { get; set; }
        public string PropertyMapPinDinamicSelection { get; set; }
        public string PropertyMapPinDinamicBackground { get; set; }
        public string GetAllPinsBackgroundColor { get; set; }
        public string PropertyTitleMapCard { get; set; }
        public string PropertyDescriptionMapCard { get; set; }
        public string FirstPropertyTitle { get; set; }
        public string FirstPropertyDescription { get; set; }
    }
}

