﻿using ScheduleCore.Models.Old;

namespace ScheduleCore.Models
{
    public class LessonNew
    {
        public int Id { get; set; }
        public string Classroom { get; set; }
        public required string Type { get; set; }
        public string Time { get; set; }
        public required string Name { get; set; }
        public TypeOfWeek Week { get; set; }


        //public string? CardDay { get; set; }
        //public string? CardProf { get; set; }
    }
}
