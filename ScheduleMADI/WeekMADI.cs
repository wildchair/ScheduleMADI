namespace ScheduleMADI
{
    internal static class WeekMADI
    {
        public static string Week
        {
            get => week; 
            set
            { 
                week = value;
                //weekLoadDate = DateTime.Now.Date;//
            }
        }
        private static string week;//текущая неделя
        //private static DateTime weekLoadDate;///

        public static string WeekByDate(DateTime now_date,DateTime date)
        {
            //var now_date = DateTime.Now.Date;

            //if (0 != DateTime.Compare(now_date, weekLoadDate))//протестировать в полночь на понедельник,
            //    Week = WeekByDate(now_date);

            string weekByDate = Week;
            

            while (0 != DateTime.Compare(now_date, date))
            {

                if (0 > DateTime.Compare(now_date, date))
                    now_date = now_date.AddDays(1);
                else
                    now_date = now_date.AddDays(-1);

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
            }
            return weekByDate;
        }
    }
}
