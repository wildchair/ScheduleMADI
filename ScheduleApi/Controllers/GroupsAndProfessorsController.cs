using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsAndProfessorsController : ControllerBase
    {
        private readonly ILogger<GroupsAndProfessorsController> _logger;
        private readonly IGroupsService _groupsService;
        private readonly IProfessorsService _professorsService;

        public GroupsAndProfessorsController(ILogger<GroupsAndProfessorsController> logger, IGroupsService service, IProfessorsService professorsService)
        {
            _logger = logger;
            _groupsService = service;
            _professorsService = professorsService;
        }

        [HttpGet(Name = "GetGroupsAndProfessors")]
        public async Task<MadiEntityRegistry> Get()
        {
            var groups = (await _groupsService.GetGroupsAsync()).Registry;
            var professors = (await _professorsService.GetProfessorsAsync()).Registry;

            return new() { Registry = groups.Concat(professors).ToDictionary(x => x.Key, x => x.Value) };
        }
    }
}