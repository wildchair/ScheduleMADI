using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models;
using ScheduleCore.Models.Old;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataBaseInitController : ControllerBase
    {
        private readonly ILogger<DataBaseInitController> _logger;
        private readonly IDataBaseService _dataBaseService;

        public DataBaseInitController(ILogger<DataBaseInitController> logger, IDataBaseService dataBaseService )
        {
            _logger = logger;
            _dataBaseService = dataBaseService;
        }

        [HttpGet()]
        public async Task<int> Get()
        {
            return await _dataBaseService.InitDbAsync();
        }
    }
}
