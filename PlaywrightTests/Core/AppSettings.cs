using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Core
{
    class AppSettings
    {
        public static string BaseUrl => Settings.Instance.CustomSettings.App.BaseUrl;

        public class Locators
        {
            public static string ExpandFilters => Settings.Instance.CustomSettings.Locators.Filters.ExpandFilters;
            public static string ExpandExtraFilters => Settings.Instance.CustomSettings.Locators.Filters.ExpandExtraFilters;
            public static string FillInputUnderLabel => Settings.Instance.CustomSettings.Locators.Filters.FillInputUnderLabel;
            public static string SearchButton => Settings.Instance.CustomSettings.Locators.Filters.SearchButton;
            public static string AddGuestsBasedOnGuestType => Settings.Instance.CustomSettings.Locators.Filters.AddGuestsBasedOnGuestType;
            public static string CalendarDaySelector => Settings.Instance.CustomSettings.Locators.Filters.CalendarDaySelector;
            public static string ExpandGuestsFilter => Settings.Instance.CustomSettings.Locators.Filters.ExpandGuestsFilter;
            public static string FilterValues => Settings.Instance.CustomSettings.Locators.Filters.FilterValues;
            public static string NumberOfPropertiesInFirstPage => Settings.Instance.CustomSettings.Locators.Filters.NumberOfPropertiesInFirstPage;
            public static string IterateEachFirstPageProperty => Settings.Instance.CustomSettings.Locators.Filters.IterateEachFirstPageProperty;
            public static string PropertyAllowedGuests => Settings.Instance.CustomSettings.Locators.Filters.PropertyAllowedGuests;
            public static string FilterNumberOfBedrooms => Settings.Instance.CustomSettings.Locators.Filters.FilterNumberOfBedrooms;
            public static string AmenititesShowMore => Settings.Instance.CustomSettings.Locators.Filters.AmenititesShowMore;
            public static string AmenititesSelection => Settings.Instance.CustomSettings.Locators.Filters.AmenititesSelection;
            public static string ApplyExtraFiltersButton => Settings.Instance.CustomSettings.Locators.Filters.ApplyExtraFiltersButton;
            public static string NumberOfBedroomsInDashboard => Settings.Instance.CustomSettings.Locators.Filters.NumberOfBedroomsInDashboard;
            public static string OpenAllPropertyAmenities => Settings.Instance.CustomSettings.Locators.Filters.OpenAllPropertyAmenities;
            public static string AllFacilitiesAmenitites => Settings.Instance.CustomSettings.Locators.Filters.AllFacilitiesAmenitites;
            public static string TranslationButtonModal => Settings.Instance.CustomSettings.Locators.Filters.TranslationButtonModal;
            public static string TranslationToggle => Settings.Instance.CustomSettings.Locators.Filters.TranslationToggle;
            public static string PropertyMapPinDinamicSelection => Settings.Instance.CustomSettings.Locators.Filters.PropertyMapPinDinamicSelection;
            public static string PropertyMapPinDinamicBackground => Settings.Instance.CustomSettings.Locators.Filters.PropertyMapPinDinamicBackground;
            public static string GetAllPinsBackgroundColor => Settings.Instance.CustomSettings.Locators.Filters.GetAllPinsBackgroundColor;
            public static string PropertyTitleMapCard => Settings.Instance.CustomSettings.Locators.Filters.PropertyTitleMapCard;
            public static string PropertyDescriptionMapCard => Settings.Instance.CustomSettings.Locators.Filters.PropertyDescriptionMapCard;
            public static string FirstPropertyTitle => Settings.Instance.CustomSettings.Locators.Filters.FirstPropertyTitle;
            public static string FirstPropertyDescription => Settings.Instance.CustomSettings.Locators.Filters.FirstPropertyDescription;
        }
    }
}
