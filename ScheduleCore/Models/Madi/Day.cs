using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleCore.Models.Madi
{
    [Owned]
    public class Day : INotifyPropertyChanged
    {
        private ObservableCollection<Class> lessons;
        private DayOfWeek name;
        private string? typeOfWeek;

        public ObservableCollection<Class> Lessons
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
            Lessons = new ObservableCollection<Class>();
            Name = name;
        }
        public Day(DayOfWeek name, ObservableCollection<Class> lessons) : this(name)
        {
            Lessons = lessons;
        }
        public Day(DayOfWeek name, ObservableCollection<Class> lessons, string typeOfWeek) : this(name, lessons)
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