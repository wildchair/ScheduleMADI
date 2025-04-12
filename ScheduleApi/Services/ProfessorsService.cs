using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models.Madi;

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
            var content = /*new FormUrlEncodedContent(*/new Dictionary<string, string>
            {
                { "step_no", "1" },
                { "task_id", "8" }
            }/*)*/;

            var html = await _apiClient.PostAsync("tplan/tasks/task8_prepview.php", content, ApiClient.ContentType.FormUrlEncoded);

            return _parser.ParseProfessors(html);
        }
    }
}