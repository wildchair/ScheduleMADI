using ScheduleCore.Models;

namespace ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces
{
    public interface IParser
    {
        public TypeOfWeek ParseWeek(string json);
        public MadiEntityRegistry ParseGroups(string html);
        public MadiEntityRegistry ParseProfessors(string html);
        public List<Day> ParseSchedule(string html);
        public List<Exam> ParseExamSchedule(string html);
    }
}