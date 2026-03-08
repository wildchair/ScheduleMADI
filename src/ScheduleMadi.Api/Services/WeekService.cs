using ScheduleApi.ServiceRegistrator;
using ScheduleMadi.Api.Services.Interfaces;
using ScheduleMadi.Core.ApiClient;
using ScheduleMadi.Core.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleMadi.Core.Models;

namespace ScheduleMadi.Api.Services
{
    [Service(ServiceLifetime.Scoped)]
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