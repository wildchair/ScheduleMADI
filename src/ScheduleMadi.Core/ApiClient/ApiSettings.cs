namespace ScheduleMadi.Core.ApiClient
{
    public class ApiSettings
    {
        public string BaseUrl { get; init; } = string.Empty;
    }

    public class UniversityApiSettings : ApiSettings { }
    public class ScheduleApiSettings : ApiSettings { }
}