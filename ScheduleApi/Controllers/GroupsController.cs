using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ILogger<GroupsController> _logger;
        private readonly IGroupsService _groupsService;

        public GroupsController(ILogger<GroupsController> logger, IGroupsService service)
        {
            _logger = logger;
            _groupsService = service;
        }

        [HttpGet(Name = "GetGroups")]
        public async Task<MadiEntityRegistry> Get()
        {
            return await _groupsService.GetGroupsAsync();
        }
    }
}