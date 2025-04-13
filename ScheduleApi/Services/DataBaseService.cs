using Microsoft.EntityFrameworkCore;
using ScheduleApi.Repository;
using ScheduleApi.ServiceRegistrator;
using ScheduleApi.Services.Interfaces;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models;

namespace ScheduleApi.Services
{
    [Injectable(ServiceLifetime.Scoped)]
    public class DataBaseService : IDataBaseService
    {
        private readonly ILogger<DataBaseService> _logger;
        private readonly UniversityApiClient _apiClient;
        private readonly IParser _parser;

        private readonly IGroupsService _groupsService;
        private readonly IProfessorsService _professorsService;
        private readonly IScheduleService _scheduleService;

        private readonly InMemoryDbContext _dbContext;
        private readonly InMemoryDbMadiContext _dbMadiContext;

        public DataBaseService(ILogger<DataBaseService> logger,
                               UniversityApiClient apiClient,
                               IParser parser,
                               IGroupsService groupsService,
                               IScheduleService scheduleService,
                               IProfessorsService professorsService,
                               InMemoryDbContext dbContext,
                               InMemoryDbMadiContext dbMadiContext)
        {
            _logger = logger;
            _apiClient = apiClient;
            _parser = parser;
            _scheduleService = scheduleService;
            _groupsService = groupsService;
            _dbContext = dbContext;
            _professorsService = professorsService;
            _dbMadiContext = dbMadiContext;
        }

        public async Task<int> InitDbAsync()
        {
            var groups = await _groupsService.GetGroupsAsync();
            var professors = await _professorsService.GetProfessorsAsync();

            foreach (var group in groups.Registry)
            {
                if (!await _dbContext.Groups.AnyAsync(g => g.Id == group.Key))
                    await _dbContext.Groups.AddAsync(new() { Id = group.Key, Name = group.Value, Lessons = new() });

                await _scheduleService.GetScheduleAsync(group.Key);
            }

            foreach (var professor in professors.Registry)
            {
                if (!await _dbContext.Professors.AnyAsync(p => p.Id == professor.Key))
                    await _dbContext.Professors.AddAsync(new() { Id = professor.Key, Name = professor.Value, Lessons = new() });

                await _scheduleService.GetScheduleAsync(professor.Key);
            }

            foreach (var group in await _dbContext.Groups.ToListAsync())
            {
                var rawGroup = await _dbMadiContext.Schedules.FindAsync(group.Id);

                foreach(var day in rawGroup.Days)
                {
                    foreach(var rawLesson in day.Lessons)
                    {
                        var lesson = new Lesson()
                        {
                            Day = day.Name,
                            Classroom = rawLesson.Classroom,
                            Type = rawLesson.Type,
                            Week = rawLesson.TypeOfWeek switch
                            {
                                "Числитель" => TypeOfWeek.Numerator,
                                "Знаменатель" => TypeOfWeek.Denominator,
                                _ => TypeOfWeek.None
                            },
                            Name = rawLesson.Name,
                            Groups = [],
                            Professors = []
                        };

                        var dbLesson = await _dbContext.Lessons.SingleOrDefaultAsync(x => x.Name == lesson.Name &&
                                                                   x.End == lesson.End &&
                                                                   x.Week == lesson.Week &&
                                                                   x.Classroom == lesson.Classroom &&
                                                                   x.Day == lesson.Day &&
                                                                   x.Start == lesson.Start &&
                                                                   x.Type == lesson.Type);

                        if (dbLesson != default)
                        {
                            group.Lessons.Add(dbLesson);
                            dbLesson.Groups.Add(group);
                        }
                        else
                        {
                            group.Lessons.Add(lesson);
                            lesson.Groups.Add(group);
                        }

                    }
                }
            }

            foreach (var professor in await _dbContext.Professors.ToListAsync())
            {
                var rawGroup = await _dbMadiContext.Schedules.FindAsync(professor.Id);

                foreach (var day in rawGroup.Days)
                {
                    foreach (var rawLesson in day.Lessons)
                    {
                        var lesson = new Lesson()
                        {
                            Day = day.Name,
                            Classroom = rawLesson.Classroom,
                            Type = rawLesson.Type,
                            Week = rawLesson.TypeOfWeek switch
                            {
                                "Числитель" => TypeOfWeek.Numerator,
                                "Знаменатель" => TypeOfWeek.Denominator,
                                _ => TypeOfWeek.None
                            },
                            Name = rawLesson.Name,
                            Groups = [],
                            Professors = []
                        };

                        var dbLesson = await _dbContext.Lessons.SingleOrDefaultAsync(x => x.Name == lesson.Name &&
                                                                   x.End == lesson.End &&
                                                                   x.Week == lesson.Week &&
                                                                   x.Classroom == lesson.Classroom &&
                                                                   x.Day == lesson.Day &&
                                                                   x.Start == lesson.Start &&
                                                                   x.Type == lesson.Type);

                        if (dbLesson != default)
                        {
                            professor.Lessons.Add(dbLesson);
                            dbLesson.Professors.Add(professor);
                        }
                        else
                        {
                            _dbContext.Lessons.Add(lesson);
                            professor.Lessons.Add(lesson);
                            lesson.Professors.Add(professor);
                        }

                    }
                }
            }

            return await _dbContext.SaveChangesAsync();
        }
    }
}
