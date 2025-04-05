using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.MadiSiteApiHelpers;
using ScheduleCore.MadiSiteApiHelpers.Parsers;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models;

namespace ScheduleApi.Services
{
    [Injectable(ServiceLifetime.Scoped)]
    public class WeekService : IWeekService
    {
        private readonly ILogger<WeekService> _logger;
        private readonly ApiClient _apiClient;
        private readonly IParser _parser;

        public WeekService(ILogger<WeekService> logger, ApiClient apiClient, IParser parser)
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

            var json = await _apiClient.FetchWeekAsync(content, CancellationToken.None);

            return _parser.ParseWeek(json);
        }
    }
}