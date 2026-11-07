using System.Collections.ObjectModel;
using ScheduleMADI;

namespace ScheduleMadi.Tests;

public class ScheduleCarouselTests
{
    private static readonly DateTime Today = new(2026, 10, 28);

    private static CarouselVM CreateCarousel() =>
        new(new DateTime(2026, 9, 28), new DateTime(2026, 12, 28), Today);

    private static List<Day> CreateSchedule(string group)
    {
        return new[] { "Числитель", "Знаменатель" }
            .SelectMany(week => Enum.GetValues<DayOfWeek>().Select(day =>
                new Day(day, new ObservableCollection<Lesson>
                {
                    new() { CardName = $"{group}: {week}: {day}" }
                }, week)))
            .ToList();
    }

    [Fact]
    public void InitialDateAndPositionReferToToday()
    {
        var carousel = CreateCarousel();

        Assert.Equal(Today, carousel.SelectedDate);
        Assert.Equal(Today, carousel.Days[carousel.Position].Date);
        Assert.Equal(new DateTime(2026, 9, 28), carousel.Days[0].Date);
        Assert.Equal(new DateTime(2026, 12, 28), carousel.Days[^1].Date);
    }

    [Fact]
    public void SwipeAcrossSundayChangesDateAndWeek()
    {
        WeekMADI.Week = "Числитель";
        var carousel = CreateCarousel();
        carousel.SetSchedule(CreateSchedule("А"), Today);
        carousel.SelectedDate = new DateTime(2026, 11, 1);

        Assert.Equal("А: Числитель: Sunday", carousel.Days[carousel.Position].Lessons.Single().CardName);
        carousel.Position++;

        Assert.Equal(new DateTime(2026, 11, 2), carousel.SelectedDate);
        Assert.Equal("А: Знаменатель: Monday", carousel.Days[carousel.Position].Lessons.Single().CardName);
        carousel.Position--;
        Assert.Equal(new DateTime(2026, 11, 1), carousel.SelectedDate);
    }

    [Fact]
    public void CalendarJumpUsesTheCorrectWeekBeforeAndAfterToday()
    {
        WeekMADI.Week = "Числитель";
        var carousel = CreateCarousel();
        carousel.SetSchedule(CreateSchedule("А"), Today);

        carousel.SelectedDate = Today.AddDays(-7);
        Assert.Equal("А: Знаменатель: Wednesday", carousel.Days[carousel.Position].Lessons.Single().CardName);
        carousel.SelectedDate = Today.AddDays(14);
        Assert.Equal("А: Числитель: Wednesday", carousel.Days[carousel.Position].Lessons.Single().CardName);
    }

    [Fact]
    public void RefreshingOrChangingGroupKeepsTheSelectedDateAndPages()
    {
        WeekMADI.Week = "Числитель";
        var carousel = CreateCarousel();
        var pages = carousel.Days;
        carousel.SetSchedule(CreateSchedule("А"), Today);
        carousel.SelectedDate = Today.AddDays(8);

        carousel.SetSchedule(Array.Empty<Day>(), Today);
        Assert.All(carousel.Days, page => Assert.Empty(page.Lessons));
        carousel.SetSchedule(CreateSchedule("Б"), Today);

        Assert.Same(pages, carousel.Days);
        Assert.Equal(Today.AddDays(8), carousel.SelectedDate);
        Assert.Equal("Б: Знаменатель: Thursday", carousel.Days[carousel.Position].Lessons.Single().CardName);
    }

    [Theory]
    [InlineData(2026, 11, 30)]
    [InlineData(2026, 11, 8)]
    [InlineData(2026, 12, 1)]
    public void CalendarSelectionAndBothNeighborsKeepMatchingPages(int year, int month, int day)
    {
        WeekMADI.Week = "Числитель";
        var carousel = CreateCarousel();
        carousel.SetSchedule(CreateSchedule("А"), Today);
        var selected = new DateTime(year, month, day);
        foreach (var direction in new[] { -1, 1 })
        {
            carousel.SelectedDate = selected;
            carousel.Position += direction;
            var expected = selected.AddDays(direction);
            Assert.Equal(expected, carousel.SelectedDate);
            Assert.Equal(expected, carousel.Days[carousel.Position].Date);
            Assert.Contains(expected.DayOfWeek.ToString(), carousel.Days[carousel.Position].Lessons.Single().CardName);
        }
    }

    [Fact]
    public void NativeInvalidPositionsDoNotMoveTheCalendar()
    {
        var carousel = CreateCarousel();

        carousel.Position = -1;
        carousel.Position = carousel.Days.Count;

        Assert.Equal(Today, carousel.SelectedDate);
    }

    [Fact]
    public void CalendarIsLimitedToTheExistingDateRange()
    {
        var carousel = CreateCarousel();

        carousel.SelectedDate = carousel.MinDate.AddDays(-1);
        Assert.Equal(0, carousel.Position);
        carousel.SelectedDate = carousel.MaxDate.AddDays(1);
        Assert.Equal(carousel.Days.Count - 1, carousel.Position);
    }

    [Fact]
    public void YearBoundaryDoesNotChangeWeekUntilMonday()
    {
        WeekMADI.Week = "Числитель";
        var reference = new DateTime(2026, 12, 30);
        var carousel = new CarouselVM(reference.AddDays(-1), reference.AddDays(7), reference);
        carousel.SetSchedule(CreateSchedule("А"), reference);

        carousel.SelectedDate = new DateTime(2027, 1, 1);
        Assert.Equal("А: Числитель: Friday", carousel.Days[carousel.Position].Lessons.Single().CardName);
        carousel.SelectedDate = new DateTime(2027, 1, 4);
        Assert.Equal("А: Знаменатель: Monday", carousel.Days[carousel.Position].Lessons.Single().CardName);
    }
}
