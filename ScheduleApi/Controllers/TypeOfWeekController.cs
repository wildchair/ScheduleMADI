using Microsoft.AspNetCore.Mvc;
using ScheduleCore.Models;
using ScheduleCore.Parser;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeOfWeekController : ControllerBase
    {
        private readonly ILogger<TypeOfWeekController> _logger;

        public TypeOfWeekController(ILogger<TypeOfWeekController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTypeOfWeek")]
        public async Task<TypeOfWeek> Get()
        {
            return await ParseMADI.GetWeek(CancellationToken.None);
        }
    }
}