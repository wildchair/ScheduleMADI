using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleMADI
{
    public class DayView
    {
        public ObservableCollection<SubjectCard> Cards { get => cards; set => cards = value; }
        private ObservableCollection<SubjectCard> cards = new();
        public DayView() { }
        public DayView(ObservableCollection<SubjectCard> cards) 
        { 
            this.Cards = cards;
        }
    }
}
