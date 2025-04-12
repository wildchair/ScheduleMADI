using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models.Madi;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleService _scheduleService;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleService scheduleService)
        {
            _logger = logger;
            _scheduleService = scheduleService;
        }

        [HttpGet("{id}")]
        public async Task<Schedule> Get(int id)
        {
            return await _scheduleService.GetScheduleAsync(id);
        }
    }
}