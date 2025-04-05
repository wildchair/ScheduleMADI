using Microsoft.AspNetCore.Mvc;
using ScheduleCore.Models;
using ScheduleCore.Parser;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostSchedule")]
        public async Task<IEnumerable<Day>> Post(int id)
        {
            return await ParseMADI.GetSchedule(id, CancellationToken.None);
        }
    }
}
