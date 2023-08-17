using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleMADI
{
    public class WithoutCarouselVM : INotifyPropertyChanged
    {
        public Day CurrentDay//отображаемый день
        {
            get => currentDay;
            set
            {
                if (currentDay != value)
                {
                    currentDay = value;
                    OnPropertyChanged();
                }
            }
        }
        private Day currentDay;

        public int TapNums
        {
            get => tapNums;
            set
            {
                if (value != tapNums)
                {
                    tapNums = value;
                    OnPropertyChanged();
                }
            }
        }
        private int tapNums = 0;

        private DateTime today = DateTime.Now.Date;

        public DateTime DatepickerDate
        {
            get => datepickerDate;
            set
            {
                if (datepickerDate != value)
                {
                    CurrentDay = mainPageVM.Schedule.Find(x => x.Name == value.DayOfWeek && x.TypeOfWeek == WeekMADI.WeekByDate(today, value.Date));
                    datepickerDate = value;
                    OnPropertyChanged();
                }
            }
        }//текущая дата в датапикере
        private DateTime datepickerDate = DateTime.Now.Date;

        private readonly MainPageVM mainPageVM;

        public event PropertyChangedEventHandler PropertyChanged;

        public WithoutCarouselVM(MainPageVM mainPageVM)
        {
            mainPageVM.PropertyChanged += OnScheduleChanged;
            this.mainPageVM = mainPageVM;
        }

        private void OnScheduleChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(mainPageVM.Schedule))
            {
                if (mainPageVM.Schedule == null)
                {
                    CurrentDay = null;
                    return;
                }
                TapNums = 0;
                DatepickerDate = today.Date;
                CurrentDay = mainPageVM.Schedule.Find(x => x.Name == today.DayOfWeek && x.TypeOfWeek == WeekMADI.Week);
                TapNums = 2;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
