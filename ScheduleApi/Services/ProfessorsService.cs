using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models;

namespace ScheduleApi.Services
{
    [Injectable(ServiceLifetime.Scoped)]
    public class ProfessorsService : IProfessorsService
    {
        private readonly ILogger<ProfessorsService> _logger;
        private readonly UniversityApiClient _apiClient;
        private readonly IParser _parser;

        public ProfessorsService(ILogger<ProfessorsService> logger, UniversityApiClient apiClient, IParser parser)
        {
            _logger = logger;
            _apiClient = apiClient;
            _parser = parser;
        }
        public async Task<MadiEntityRegistry> GetProfessorsAsync()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "step_no", "1" },
                { "task_id", "8" }
            });

            ScheduleCore.MadiSiteApiHelpers.ApiClient client = new("http://raspisanie.madi.ru/tplan/");
            var html1 = await client.FetchProfessorsAsync(content, CancellationToken.None);
            var html = await _apiClient.PostAsync("tplan/tasks/task8_prepview.php", content);

            return _parser.ParseProfessors(html);
        }
    }
}