using ScheduleCore.Models.Madi;

namespace ScheduleCore.MadiSiteApiHelpers.Utils
{
    public static class WeekTypeCalculator
    {
        public static TypeOfWeek WeekByDate(DateTime desiredDate, TypeOfWeek knownWeekType, DateTime knownDate)
        {
            var calculatedWeekType = knownWeekType;


            while (0 != DateTime.Compare(desiredDate, knownDate))
            {
                if (0 > DateTime.Compare(desiredDate, knownDate))
                {
                    if (desiredDate.DayOfWeek == DayOfWeek.Sunday)
                        switch (calculatedWeekType)
                        {
                            case TypeOfWeek.Numerator:
                                calculatedWeekType = TypeOfWeek.Denominator;
                                break;
                            case TypeOfWeek.Denominator:
                                calculatedWeekType = TypeOfWeek.Numerator;
                                break;
                        }
                    desiredDate = desiredDate.AddDays(1);
                }
                else
                {
                    if (desiredDate.DayOfWeek == DayOfWeek.Monday)
                        switch (calculatedWeekType)
                        {
                            case TypeOfWeek.Numerator:
                                calculatedWeekType = TypeOfWeek.Denominator;
                                break;
                            case TypeOfWeek.Denominator:
                                calculatedWeekType = TypeOfWeek.Numerator;
                                break;
                        }
                    desiredDate = desiredDate.AddDays(-1);
                }
            }

            return calculatedWeekType;
        }
    }
}
