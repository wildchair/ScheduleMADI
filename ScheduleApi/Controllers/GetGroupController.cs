using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Repository;
using ScheduleCore.Models;
using ScheduleCore.Models.DTO;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetGroupController : ControllerBase
    {
        private readonly ILogger<GetGroupController> _logger;
        private readonly InMemoryDbContext _dbContext;

        public GetGroupController(ILogger<GetGroupController> logger, InMemoryDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<Group> Get(int id)
        {
            var group =  await _dbContext.Groups.Include(x => x.Lessons).ThenInclude(x => x.Professors).FirstAsync(x => x.Id == id);
            return group;
        }
    }
}