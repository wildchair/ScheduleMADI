using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.MadiSiteApiHelpers;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models;

namespace ScheduleApi.Services
{
    [Injectable(ServiceLifetime.Scoped)]
    public class GroupsService : IGroupsService
    {
        private readonly ILogger<GroupsService> _logger;
        private readonly ApiClient _apiClient;
        private readonly IParser _parser;

        public GroupsService(ILogger<GroupsService> logger, ApiClient apiClient, IParser parser)
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

            var html = await _apiClient.FetchGroupsAsync(content, CancellationToken.None);

            return _parser.ParseGroups(html);
        }
    }
}