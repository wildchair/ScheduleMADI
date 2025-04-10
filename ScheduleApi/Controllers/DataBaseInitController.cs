using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataBaseInitController : ControllerBase
    {
        private readonly ILogger<DataBaseInitController> _logger;
        private readonly IDataBaseService _dataBaseService;

        public DataBaseInitController(ILogger<DataBaseInitController> logger, IDataBaseService dataBaseService)
        {
            _logger = logger;
            _dataBaseService = dataBaseService;
        }

        [HttpGet(Name = "LoadDb")]
        public async Task<int> Get()
        {
            return await _dataBaseService.InitDbAsync();
        }
    }
}
