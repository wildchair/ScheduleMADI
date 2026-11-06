#nullable enable
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ScheduleMADI;

public sealed class ScheduleDate : INotifyPropertyChanged
{
    private readonly ObservableCollection<Lesson> emptyLessons = new();
    private ObservableCollection<Lesson> lessons;

    public ScheduleDate(DateTime date)
    {
        Date = date.Date;
        lessons = emptyLessons;
    }

    public DateTime Date { get; }
    public ObservableCollection<Lesson> Lessons => lessons;

    public event PropertyChangedEventHandler? PropertyChanged;

    internal void SetLessons(ObservableCollection<Lesson>? value)
    {
        value ??= emptyLessons;
        if (ReferenceEquals(lessons, value))
            return;

        lessons = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lessons)));
    }
}
