using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleMADI
{
    public class Lesson//можно не реализовывать интерфейс, если не будет подкапотных изменений конкретно лессона
    {
#nullable enable
        public string? CardTime { get; set; }
        public string? CardDay { get; set; }
        public string? CardType { get; set; }
        public string? CardName { get; set; }
        public string? CardProf { get; set; }
        public string? CardRoom { get; set; }
#nullable disable
    }
}
