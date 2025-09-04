using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ScheduleCore.Models.RawModels
{
    public class Day : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }

        private ObservableCollection<Lesson> lessons;
        private DayOfWeek name;
        private string? typeOfWeek;

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
        public string? TypeOfWeek
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
            Name = name;
        }
        public Day(DayOfWeek name, ObservableCollection<Lesson> lessons) : this(name)
        {
            Lessons = lessons;
        }
        public Day(DayOfWeek name, ObservableCollection<Lesson> lessons, string typeOfWeek) : this(name, lessons)
        {
            TypeOfWeek = typeOfWeek;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}