namespace ScheduleCore.Parser
{
    internal static class WeekMADI
    {
        public static string Week
        {
            get => week; 
            set
            { 
                week = value;
            }
        }
        private static string week;//текущая неделя

        public static string WeekByDate(DateTime now_date,DateTime date)
        {
            string weekByDate = Week;
            

            while (0 != DateTime.Compare(now_date, date))
            {
                if (0 > DateTime.Compare(now_date, date))
                {
                    if (now_date.DayOfWeek == DayOfWeek.Sunday)
                        switch (weekByDate)
                        {
                            case "Числитель":
                                weekByDate = "Знаменатель";
                                break;
                            case "Знаменатель":
                                weekByDate = "Числитель";
                                break;
                        }
                    now_date = now_date.AddDays(1);
                }
                else
                {
                    if (now_date.DayOfWeek == DayOfWeek.Monday)
                        switch (weekByDate)
                        {
                            case "Числитель":
                                weekByDate = "Знаменатель";
                                break;
                            case "Знаменатель":
                                weekByDate = "Числитель";
                                break;
                        }
                    now_date = now_date.AddDays(-1);
                }                
            }
            return weekByDate;
        }
    }
}
