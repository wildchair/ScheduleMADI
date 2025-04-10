using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Repository;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models;
using ScheduleCore.Models.Old;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataBaseGetController : ControllerBase
    {
        private readonly ILogger<DataBaseGetController> _logger;
        private readonly InMemoryDbContext _dbContext;

        public DataBaseGetController(ILogger<DataBaseGetController> logger, InMemoryDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetDb")]
        public async Task<IEnumerable<Schedule>> Get()
        {
            return await _dbContext.Schedules.Include(x=>x.Days).ThenInclude(x=>x.Lessons).ToListAsync();
        }
    }
}
