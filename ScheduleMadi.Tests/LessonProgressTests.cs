using ScheduleMADI;

namespace ScheduleMadi.Tests;

public class LessonProgressTests
{
    private static readonly DateTime Today = new(2026, 10, 28);

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(1, 0)]
    public void AdjacentPagesUseTheirOwnDate(int dayOffset, double expected)
    {
        Assert.Equal(expected, LessonProgress.Calculate(Today.AddDays(dayOffset), "09:00-10:00", Today.AddHours(9.5)));
    }

    [Theory]
    [InlineData(8, 0)]
    [InlineData(9, 0)]
    [InlineData(9.5, 0.5)]
    [InlineData(10, 1)]
    [InlineData(11, 1)]
    public void CurrentLessonProgressFollowsItsTimeRange(double hour, double expected)
    {
        Assert.Equal(expected, LessonProgress.Calculate(Today, "09:00-10:00", Today.AddHours(hour)), 6);
    }

    [Fact]
    public void AllDayCardUsesTheWholeDay()
    {
        Assert.Equal(0.5, LessonProgress.Calculate(Today, null, Today.AddHours(12)), 6);
    }

    [Theory]
    [InlineData("не время")]
    [InlineData("10:00-09:00")]
    [InlineData("09:00-09:00")]
    public void InvalidTimeDoesNotCrashTheCard(string time)
    {
        Assert.Equal(0, LessonProgress.Calculate(Today, time, Today.AddHours(9.5)));
    }
}
