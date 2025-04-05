using Microsoft.AspNetCore.Mvc;
using ScheduleCore.Parser;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorsController : ControllerBase
    {
        private readonly ILogger<ProfessorsController> _logger;

        public ProfessorsController(ILogger<ProfessorsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetProfessors")]
        public async Task<Dictionary<string, string>> Get()
        {
            return await ParseMADI.GetProfessors(CancellationToken.None);
        }
    }
}
