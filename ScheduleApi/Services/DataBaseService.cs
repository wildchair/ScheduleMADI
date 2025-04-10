using Microsoft.EntityFrameworkCore;
using ScheduleApi.Repository;
using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models;
using ScheduleCore.Models.Old;

namespace ScheduleApi.Services
{
    [Injectable(ServiceLifetime.Scoped)]
    public class DataBaseService : IDataBaseService
    {
        private readonly ILogger<DataBaseService> _logger;
        private readonly UniversityApiClient _apiClient;
        private readonly IParser _parser;

        private readonly IGroupsService _groupsService;
        private readonly IScheduleService _scheduleService;

        private readonly InMemoryDbContext _dbContext;

        public DataBaseService(ILogger<DataBaseService> logger,
                               UniversityApiClient apiClient,
                               IParser parser,
                               IGroupsService groupsService,
                               IScheduleService scheduleService,
                               InMemoryDbContext dbContext)
        {
            _logger = logger;
            _apiClient = apiClient;
            _parser = parser;
            _scheduleService = scheduleService;
            _groupsService = groupsService;
            _dbContext = dbContext;
        }

        public async Task<int> InitDbAsync()
        {
            var groups = await _groupsService.GetGroupsAsync();

            var schedules = new List<Schedule>();

            foreach (var group in groups.Registry)
            {
                var days = (await _scheduleService.GetScheduleAsync(group.Key)).ToArray();

                var lessonDict = new Dictionary<DayOfWeek, IEnumerable<ScheduleCore.Models.LessonNew>>();

                foreach (var day in days)
                {
                    var lessons = new List<ScheduleCore.Models.LessonNew>();
                    foreach (var lesson in day.Lessons)
                    {
                        var rightLesson = new ScheduleCore.Models.LessonNew()
                        {
                            Name = lesson.CardName,
                            Classroom = lesson.CardRoom,
                            Time = lesson.CardTime,
                            Type = lesson.CardType,
                            Week = lesson.CardDay switch
                            {
                                "Числитель" => TypeOfWeek.Numerator,
                                "Знаменатель" => TypeOfWeek.Denominator,
                                _ => TypeOfWeek.None
                            }
                        };

                        lessons.Add(rightLesson);
                    }

                    lessonDict.Add(day.Name, lessons);
                }
                var schedule = new Schedule() { Id = group.Key, Owner = group.Value, Days = lessonDict };

                _dbContext.Schedules.Add(schedule);
            }
            return await _dbContext.SaveChangesAsync();
        }
    }
}
