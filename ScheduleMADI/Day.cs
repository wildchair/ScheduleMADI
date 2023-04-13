using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleMADI
{
    internal class Day
    {
        public List<Lesson> Lessons { get; set; }
        public DayOfWeek Name { get; set; }
        public Day(DayOfWeek name)
        {
            Lessons = new List<Lesson>();
            this.Name = name;
        }
        public Day(DayOfWeek name, List<Lesson> lessons) : this(name)
        {
            this.Lessons = lessons;
        }
    }
}
