using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models;

namespace ScheduleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamScheduleController : ControllerBase
    {
        private readonly ILogger<ExamScheduleController> _logger;
        private readonly IExamScheduleService _examScheduleService;

        public ExamScheduleController(ILogger<ExamScheduleController> logger, IExamScheduleService examScheduleService)
        {
            _logger = logger;
            _examScheduleService = examScheduleService;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<Exam>> Get(int id)
        {
            return await _examScheduleService.GetExamScheduleAsync(id);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class Test : ControllerBase
    {
        private readonly ILogger<Test> _logger;

        public Test(ILogger<Test> logger, IExamScheduleService examScheduleService)
        {
            _logger = logger;
        }

        [HttpGet()]
        public async Task<string> Get()
        {
            return "Ortem was here";
        }
    }
}
