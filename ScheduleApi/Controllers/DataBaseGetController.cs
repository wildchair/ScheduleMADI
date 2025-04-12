using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Repository;
using ScheduleCore.Models;

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
        public async Task<IEnumerable<Owner>> Get()
        {
            return await _dbContext.Owners.Include(x => x.Lessons).ToListAsync();
        }
    }
}