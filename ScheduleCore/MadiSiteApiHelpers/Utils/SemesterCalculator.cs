namespace ScheduleCore.MadiSiteApiHelpers.Utils
{
    public static class SemesterCalculator
    {
        public static (int semester, int year) Calculate(DateTime date, bool forExam = false)
        {
            int semester, year;

            int lastDayToRequest;

            if (forExam)
                lastDayToRequest = 30;
            else
                lastDayToRequest = 2;

            if (date.Date.Month >= 8 || date.Date.Month == 1 && date.Date.Day < lastDayToRequest)
            {

                year = date.Year;
                semester = 1;

            }
            else
            {
                year = date.Year - 1;
                semester = 2;
            }

            if (date.Date.Month == 1 && date.Date.Day < lastDayToRequest)
                year -= 1;

            return (semester, year - 2000);
        }
    }
}
