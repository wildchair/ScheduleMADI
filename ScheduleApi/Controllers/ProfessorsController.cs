using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorsController : ControllerBase
    {
        private readonly ILogger<ProfessorsController> _logger;
        private readonly IProfessorsService _professorsService;

        public ProfessorsController(ILogger<ProfessorsController> logger, IProfessorsService professorsService)
        {
            _logger = logger;
            _professorsService = professorsService;
        }

        [HttpGet(Name = "GetProfessors")]
        public async Task<MadiEntityRegistry> Get()
        {
            return await _professorsService.GetProfessorsAsync();
        }
    }
}
