using Microsoft.AspNetCore.Mvc;
using ScheduleCore.Parser;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(ILogger<GroupsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetGroups")]
        public async Task<Dictionary<string, string>> Get()
        {
            return await ParseMADI.GetGroups(CancellationToken.None);
        }
    }
}
