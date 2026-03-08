using ScheduleMadi.Core.Models;
using ScheduleMadi.Core.Models.Madi;

namespace ScheduleMadi.Core.MadiSiteApiHelpers.Parsers.Interfaces
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