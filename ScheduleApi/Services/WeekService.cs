using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models.Madi;

namespace ScheduleApi.Services
{
    [Injectable(ServiceLifetime.Scoped)]
    public class WeekService : IWeekService
    {
        private readonly ILogger<WeekService> _logger;
        private readonly UniversityApiClient _apiClient;
        private readonly IParser _parser;

        public WeekService(ILogger<WeekService> logger, UniversityApiClient apiClient, IParser parser)
        {
            _logger = logger;
            _apiClient = apiClient;
            _parser = parser;
        }

        public async Task<TypeOfWeek> GetTypeOfWeekAsync()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
            });

            var json = await _apiClient.GetAsync("tplan/calendar.php");

            return _parser.ParseWeek(json);
        }
    }
}