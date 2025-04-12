using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models.Madi;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeekController : ControllerBase
    {
        private readonly ILogger<WeekController> _logger;
        private readonly IWeekService _weekService;

        public WeekController(ILogger<WeekController> logger, IWeekService weekService)
        {
            _logger = logger;
            _weekService = weekService;
        }

        [HttpGet(Name = "GetWeek")]
        public async Task<TypeOfWeek> Get()
        {
            return await _weekService.GetTypeOfWeekAsync();
        }
    }
}