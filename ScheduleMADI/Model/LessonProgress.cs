#nullable enable
using System.Globalization;

namespace ScheduleMADI;

public static class LessonProgress
{
    public static double Calculate(DateTime date, string? time, DateTime now)
    {
        if (date.Date < now.Date)
            return 1;
        if (date.Date > now.Date)
            return 0;

        var start = TimeSpan.Zero;
        var end = TimeSpan.FromDays(1);
        if (!string.IsNullOrWhiteSpace(time))
        {
            var parts = time.Split('-', 2, StringSplitOptions.TrimEntries);
            if (parts.Length != 2 ||
                !TimeSpan.TryParse(parts[0], CultureInfo.InvariantCulture, out start) ||
                !TimeSpan.TryParse(parts[1], CultureInfo.InvariantCulture, out end) || end <= start)
                return 0;
        }

        return Math.Clamp((now.TimeOfDay - start).TotalSeconds / (end - start).TotalSeconds, 0, 1);
    }
}
