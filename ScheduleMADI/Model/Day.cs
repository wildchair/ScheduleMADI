using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleMADI
{
    public class Day : INotifyPropertyChanged
    {
        private ObservableCollection<Lesson> lessons;
        private DayOfWeek name;
        private string typeOfWeek;

        public ObservableCollection<Lesson> Lessons
        {
            get => lessons;
            set
            {
                if (lessons != value)
                {
                    lessons = value;
                    OnPropertyChanged();//а нужно ли?
                }
            }
        }
        public DayOfWeek Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TypeOfWeek
        {
            get => typeOfWeek;
            set
            {
                if (typeOfWeek != value)
                {
                    typeOfWeek = value;
                    OnPropertyChanged();
                }
            }
        }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
