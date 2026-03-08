using Microsoft.AspNetCore.Mvc;
using ScheduleMadi.Api.Services.Interfaces;
using ScheduleMadi.Core.Models;

namespace ScheduleMadi.Api.Controllers
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