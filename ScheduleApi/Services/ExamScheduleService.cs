using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.MadiSiteApiHelpers.Utils;
using ScheduleCore.Models;

namespace ScheduleApi.Services
{
    [Injectable(ServiceLifetime.Scoped)]
    public class ExamScheduleService : IExamScheduleService
    {
        private readonly ILogger<ExamScheduleService> _logger;
        private readonly UniversityApiClient _apiClient;
        private readonly IParser _parser;
        private readonly IGroupsService _groupsService;
        private readonly IProfessorsService _professorsService;

        public ExamScheduleService(ILogger<ExamScheduleService> logger, UniversityApiClient apiClient, IParser parser, IGroupsService groupsService, IProfessorsService professorsService)
        {
            _logger = logger;
            _apiClient = apiClient;
            _parser = parser;
            _groupsService = groupsService;
            _professorsService = professorsService;
        }

        public async Task<IEnumerable<Exam>> GetExamScheduleAsync(int id)
        {
            var groups = await _groupsService.GetGroupsAsync();

            FormUrlEncodedContent content;
            var date = SemesterCalculator.Calculate(DateTime.Now, true);

            if (groups.Registry.ContainsKey(id))
            {
                content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "tab", "3" },

                    { "gp_id", $"{id}" },

                    {"tp_year", $"{date.year}" },

                    {"sem_no", $"{date.semester}" }
                });
            }
            else
            {
                content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "tab", "4" },

                    { "pr_id", $"{id}" },

                    {"tp_year", $"{date.year}" },

                    {"sem_no", $"{date.semester}" }
                });
            }

            var html = await _apiClient.PostAsync("tplan/tasks/tableFiller.php", content);

            return _parser.ParseExamSchedule(html);
        }
    }
}