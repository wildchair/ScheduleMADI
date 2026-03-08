using ScheduleApi.ServiceRegistrator;
using ScheduleMadi.Api.Services.Interfaces;
using ScheduleMadi.Core.ApiClient;
using ScheduleMadi.Core.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleMadi.Core.Models.Madi;

namespace ScheduleMadi.Api.Services
{
    [Service(ServiceLifetime.Scoped)]
    public class GroupsService : IGroupsService
    {
        private readonly ILogger<GroupsService> _logger;
        private readonly UniversityApiClient _apiClient;
        private readonly IParser _parser;

        public GroupsService(ILogger<GroupsService> logger, UniversityApiClient apiClient, IParser parser)
        {
            _logger = logger;
            _apiClient = apiClient;
            _parser = parser;
        }

        public async Task<MadiEntityRegistry> GetGroupsAsync()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "step_no", "1" },
                { "task_id", "7" }
            });

            var html = await _apiClient.PostAsync("tplan/tasks/task3,7_fastview.php", content);

            return _parser.ParseGroups(html);
        }
    }
}