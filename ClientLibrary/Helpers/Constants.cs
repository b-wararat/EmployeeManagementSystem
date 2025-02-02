namespace ClientLibrary.Helpers
{
    public class Constants
    {
        public const string AuthUrl = "api/authentication";
        public const string BranchUrl = "api/Branch";
        public const string DepartmentUrl = "api/Department";
        public const string GeneralDepartmentUrl = "api/GeneralDepartment";
        public const string CountryUrl = "api/Country";
        public const string CityUrl = "api/City";
        public const string TownUrl = "api/Town";
        public const string WeatherForecastUrl = "api/WeatherForecast";
    }
    public enum Http {
        Get,
        Post,
        Put,
        Delete
    }
}
