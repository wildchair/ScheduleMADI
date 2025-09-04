using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.Models.RawModels;

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
}