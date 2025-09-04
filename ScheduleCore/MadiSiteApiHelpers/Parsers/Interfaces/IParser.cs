
using ScheduleCore.Models.RawModels;

namespace ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces
{
    public interface IParser
    {
        public string ParseWeek(string json);
        public Dictionary<int, string> ParseGroups(string html);
        public Dictionary<int, string> ParseProfessors(string html);
        public List<Day> ParseSchedule(string html);
        public List<Exam> ParseExamSchedule(string html);
    }
}