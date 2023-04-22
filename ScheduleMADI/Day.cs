using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleMADI
{
    public class Day
    {
        public ObservableCollection<Lesson> Lessons { get; set; }
        public DayOfWeek Name { get; set; }
        public string TypeOfWeek { get; set; }
        public Day() { }
        public Day(DayOfWeek name)
        {
            Lessons = new ObservableCollection<Lesson>();
            this.Name = name;
        }
        public Day(DayOfWeek name, ObservableCollection<Lesson> lessons) : this(name)
        {
            this.Lessons = lessons;
        }
        public Day(DayOfWeek name, ObservableCollection<Lesson> lessons, string typeOfWeek) : this(name, lessons)
        {
            this.TypeOfWeek = typeOfWeek;
        }
    }
}
