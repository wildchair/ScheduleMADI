﻿namespace ScheduleMADI
{
    public static class ParseMADI
    {
        internal static List<Day> days = new();

        public static bool reloaded = false;
        public static string Week { get => week; set => week = value; }
        private static string week;//текущая неделя

        public static Dictionary<string, string> id_groups = new();//словарь групп

        public static KeyValuePair<string, string> ID = new(value: "3бАСУ1", key: "8716");
        

        public async static Task GetWeek()
        {
            HttpClient httpClient = new();

            var response = await httpClient.GetAsync("https://www.madi.ru/tplan/calendar.php");

            var responseString = await response.Content.ReadAsStringAsync();

            Week = responseString switch
            {
                "{\"aaData\":[\"\\u0437\"]}" => "Знаменатель",
                "{\"aaData\":[\"\\u0447\"]}" => "Числитель",
                _ => "Получить день не удалось",
            };
        }
        public async static Task GetGroups()
        {
            HttpClient httpClient = new();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "step_no", "1" },

                { "task_id", "7" }
            });

            var response = await httpClient.PostAsync("https://www.madi.ru/tplan/tasks/task3,7_fastview.php", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var str = responseString.Split(' ').ToList();
            str.RemoveAll(str => !str.Contains("value"));
            char[] charsToTrim = { '<', '>', '/', 'l', 'i', '"' };
            for (int i = 0; i < str.Count; i++)
            {
                str[i] = str[i].Remove(0, 7).Replace('<', ' ').Replace('>', ' ').Replace('/', ' ').Replace('l', ' ').Replace('i', ' ').Replace('"', ' ');
            }

            foreach (var item in str)
            {
                var buff = item.Split(" ").ToList();
                buff.RemoveAll(str => str.Length == 0);

                string id = buff[0], name = buff[1];
                id_groups.Add(id, name);
            }
        }
        public async static Task GetShedule(string gp_id)
        {
            days.Clear();
            days.Add(new Day(DayOfWeek.Sunday)
            { Lessons = new List<Lesson>() { new Lesson { Name = "Выходной день", Day = "Еженедельно" } } });

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "tab", "7" },

                { "gp_id", $"{gp_id}" }
            });

            HttpClient httpClient = new();
            var response = await httpClient.PostAsync("https://www.madi.ru/tplan/tasks/tableFiller.php", content);
            var responseString = await response.Content.ReadAsStringAsync();

            StringReader reader = new(responseString);

            reloaded = true;

            string buff;
            while ((buff = reader.ReadLine()) != null)
            {
                if (buff.Contains("colspan=6"))//отслеживание начала дня
                {

                    buff = ParseDataHTML(buff);
                    DayOfWeek dayOfWeek = DayOfWeek.Monday;
                    switch (buff)
                    {
                        case "Понедельник":
                            dayOfWeek = DayOfWeek.Monday;
                            break;
                        case "Вторник":
                            dayOfWeek = DayOfWeek.Tuesday;
                            break;
                        case "Среда":
                            dayOfWeek = DayOfWeek.Wednesday;
                            break;
                        case "Четверг":
                            dayOfWeek = DayOfWeek.Thursday;
                            break;
                        case "Пятница":
                            dayOfWeek = DayOfWeek.Friday;
                            break;
                        case "Суббота":
                            dayOfWeek = DayOfWeek.Saturday;
                            break;
                    }
                    var day = new Day(dayOfWeek);

                    for (int i = 0; i < 9; i++)//пропуск шапки
                        reader.ReadLine();

                    while (true)
                    {
                        var lesson = new Lesson();
                        for (int i = 0; i < 6; i++)//парсинг данных Lesson-а
                        {
                            buff = reader.ReadLine();
                            buff = ParseDataHTML(buff);
                            switch (i)
                            {
                                case 0:
                                    lesson.Time = buff;
                                    break;
                                case 1:
                                    lesson.Name = buff;
                                    break;
                                case 2:
                                    lesson.Type = buff;
                                    break;
                                case 3:
                                    lesson.Day = buff;
                                    break;
                                case 4:
                                    lesson.Room = buff;
                                    break;
                                case 5:
                                    lesson.Prof = buff;
                                    break;
                            }
                        }
                        day.Lessons.Add(lesson);

                        string scout = string.Empty;//отслеживание границы между днями
                        do
                            scout += (char)reader.Read();
                        while (!(scout.Contains("td") || scout.Contains("th")));
                        if (scout.Contains("td"))
                            continue;
                        else break;
                    }
                    days.Add(day);
                }

                if (buff.Contains("Полнодневные занятия"))
                {
                    reader.ReadLine();
                    reader.ReadLine();

                    do
                    {
                        buff = reader.ReadLine();
                        buff = ParseDataHTML(buff);
                        DayOfWeek dayOfWeek = DayOfWeek.Monday;
                        switch (buff)
                        {
                            case "Понедельник":
                                dayOfWeek = DayOfWeek.Monday;
                                break;
                            case "Вторник":
                                dayOfWeek = DayOfWeek.Tuesday;
                                break;
                            case "Среда":
                                dayOfWeek = DayOfWeek.Wednesday;
                                break;
                            case "Четверг":
                                dayOfWeek = DayOfWeek.Thursday;
                                break;
                            case "Пятница":
                                dayOfWeek = DayOfWeek.Friday;
                                break;
                            case "Суббота":
                                dayOfWeek = DayOfWeek.Saturday;
                                break;
                        }
                        var day = new Day(dayOfWeek);

                        var lesson = new Lesson();
                        reader.ReadLine();

                        buff = reader.ReadLine();
                        buff = ParseDataHTML(buff);
                        lesson.Name = buff;

                        buff = reader.ReadLine();
                        buff = ParseDataHTML(buff);
                        lesson.Day = buff;

                        day.Lessons.Add(lesson);
                        days.Add(day);
                        buff = reader.ReadLine();
                    }
                    while (!buff.Contains("</table>"));
                    break;
                }
            }
        }
        private static string? ParseDataHTML(string? data, List<string> strToDelete = null)
        {
            strToDelete ??= new() { "table class=\"timetable\"", "colspan=6", "style=\"white-space:pre-wrap\"",
                                         "<", ">", "/", "br", "th", "tr", "td", "b", "colspan=\"2\"", "rowspan=\"1\"" };
            foreach (var str in strToDelete)
                data = data.Replace(str, string.Empty).Trim();
            return data;
        }
    }
}
