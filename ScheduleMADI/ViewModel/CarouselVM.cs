#nullable enable
using System.ComponentModel;

namespace ScheduleMADI;

public sealed class CarouselVM : INotifyPropertyChanged
{
    private DateTime selectedDate;

    public CarouselVM(DateTime minDate, DateTime maxDate, DateTime initialDate)
    {
        MinDate = minDate.Date;
        MaxDate = maxDate.Date;
        if (MaxDate < MinDate)
            throw new ArgumentException("Maximum date must not precede minimum date.", nameof(maxDate));

        Days = Enumerable.Range(0, (MaxDate - MinDate).Days + 1)
            .Select(offset => new ScheduleDate(MinDate.AddDays(offset)))
            .ToList().AsReadOnly();
        selectedDate = ClampDate(initialDate);
    }

    public DateTime MinDate { get; }
    public DateTime MaxDate { get; }
    public IReadOnlyList<ScheduleDate> Days { get; }

    public DateTime SelectedDate
    {
        get => selectedDate;
        set
        {
            var date = ClampDate(value);
            if (selectedDate == date)
                return;

            selectedDate = date;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDate)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
        }
    }

    public int Position
    {
        get => (selectedDate - MinDate).Days;
        set
        {
            // При подключении нативные элементы карусели могут кратковременно сообщать некорректную позицию.
            if (value >= 0 && value < Days.Count)
                SelectedDate = Days[value].Date;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void SetSchedule(IReadOnlyList<Day>? schedule, DateTime referenceDate)
    {
        foreach (var page in Days)
        {
            var week = WeekMADI.WeekByDate(referenceDate.Date, page.Date);
            var day = schedule?.FirstOrDefault(day =>
                day.Name == page.Date.DayOfWeek && day.TypeOfWeek == week);
            page.SetLessons(day?.Lessons);
        }
    }

    private DateTime ClampDate(DateTime value) =>
        value.Date < MinDate ? MinDate : value.Date > MaxDate ? MaxDate : value.Date;
}
