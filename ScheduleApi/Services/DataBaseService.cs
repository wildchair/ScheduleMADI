using ScheduleApi.Repository;
using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models;
using ScheduleCore.Models.Madi;

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

            foreach (var group in groups.Registry)
            {
                var groupId = group.Key;
                var groupName = group.Value;
                var lessons = new List<Lesson>();

                Schedule schedule;
                try
                {
                    schedule = await _scheduleService.GetScheduleAsync(group.Key);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка в группах че-то крч");
                    continue;
                }

                foreach (var day in schedule.Days)
                {

                    foreach (var lesson in day.Lessons)
                    {
                        var rightLesson = new Lesson()
                        {
                            Subject = lesson.Name,
                            Classroom = lesson.Classroom,
                            Day = day.Name,
                            Type = lesson.Type,
                            Period = lesson.Day switch
                            {
                                "Числитель" => TypeOfWeek.Numerator,
                                "Знаменатель" => TypeOfWeek.Denominator,
                                _ => TypeOfWeek.None
                            }
                        };

                        lessons.Add(rightLesson);
                    }
                }
                _dbContext.Owners.Add(new() { Id = schedule.Id, Lessons = lessons, Name = schedule.Owner });
            }
            return await _dbContext.SaveChangesAsync();
        }
    }
}
