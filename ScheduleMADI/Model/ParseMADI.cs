using System.Collections.ObjectModel;

namespace ScheduleMADI
{
    public static class ParseMADI
    {
        public static Dictionary<string, string> id_groups = new();//словарь групп

        public async static Task GetWeek()
        {
            HttpClient httpClient = new();

            var response = await httpClient.GetAsync("https://raspisanie.madi.ru/tplan/calendar.php");

            var responseString = await response.Content.ReadAsStringAsync();

            WeekMADI.Week = responseString switch
            {
                "{\"aaData\":[\"\\u0437\"]}" => "Знаменатель",
                "{\"aaData\":[\"\\u0447\"]}" => "Числитель",
                _ => "Получить день не удалось",
            };
            IdMADI.BufferedDay = new KeyValuePair<DateTime, string>(DateTime.Now.Date, WeekMADI.Week);
        }
        public async static Task GetGroups()
        {
            HttpClient httpClient = new();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "step_no", "1" },

                { "task_id", "7" }
            });

            var response = await httpClient.PostAsync("https://raspisanie.madi.ru/tplan/tasks/task3,7_fastview.php", content);
            var responseString = await response.Content.ReadAsStringAsync();

            StringReader reader = new(responseString);

            for (int i = 0; i < 11; i++)
                reader.ReadLine();
            var list_buff = reader.ReadLine().Split("</li>").ToList();
            list_buff.RemoveAll(x => x == string.Empty);
            reader.Close();

            foreach (var buff in list_buff)
            {
                var group_buff = CutHTML(buff).Trim().Split("\"").ToList();
                group_buff.RemoveAll(x => x == string.Empty);
                try//в полученном html могут быть айди группы без имен
                {
                    var id = group_buff[0].Trim();
                    var name = group_buff[1].Replace(" ", String.Empty);
                    if (!id_groups.ContainsKey(id))
                        id_groups.Add(id, name);
                }
                catch (ArgumentOutOfRangeException) { continue; }
            }
        }
        public async static Task<List<Day>> GetSchedule(string gp_id)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "tab", "7" },

                { "gp_id", $"{gp_id}" }
            });

            HttpClient httpClient = new();
            var response = await httpClient.PostAsync("https://raspisanie.madi.ru/tplan/tasks/tableFiller.php", content);

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString == "Извините, по данным атрибутам информация не найдена. Пожалуйста, укажите другие атрибуты")
                throw new ParseMADIException("На сайте сейчас нет данных об этой группе.");
            else
            {
                IdMADI.BufferedSchedule = responseString;
                return ParseHTML(responseString);
            }
        }

        public async static Task<List<Day>> GetScheduleFromHTML(string html)
        {
            return ParseHTML(html);
        }

        private static List<Day> ParseHTML(string html)//парсинг html-таблицы расписания
        {
            List<Day> days = new()
            {
                new Day(DayOfWeek.Monday), new Day(DayOfWeek.Tuesday),
                new Day(DayOfWeek.Wednesday), new Day(DayOfWeek.Thursday),
                new Day(DayOfWeek.Friday), new Day(DayOfWeek.Saturday),
                new Day(DayOfWeek.Sunday) {Lessons = new ObservableCollection<Lesson>()
                { new Lesson { CardName = "Выходной день", CardDay = "Еженедельно" } } }
            };

            StringReader reader = new(html);

            foreach (var day in days)//это че такое вообще?
                if (day.Name != DayOfWeek.Sunday)
                    day.Lessons.Clear();//как это родилось?

            string buff;
            while ((buff = reader.ReadLine()) != null)
            {
                if (buff.Contains("colspan=6"))//отслеживание начала дня
                {
                    //проход по дню
                    buff = CutHTML(buff);
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
                    var day = days.Find(x => x.Name == dayOfWeek);

                    for (int i = 0; i < 9; i++)//пропуск шапки
                        reader.ReadLine();

                    while (true)
                    {
                        var lesson = new Lesson();
                        for (int i = 0; i < 6; i++)//парсинг данных Lesson-а
                        {
                            buff = reader.ReadLine();
                            buff = CutHTML(buff);
                            switch (i)
                            {
                                case 0:
                                    lesson.CardTime = buff;
                                    break;
                                case 1:
                                    lesson.CardName = buff;
                                    break;
                                case 2:
                                    lesson.CardType = buff;
                                    break;
                                case 3:
                                    lesson.CardDay = buff;
                                    break;
                                case 4:
                                    lesson.CardRoom = buff;
                                    break;
                                case 5:
                                    var a = buff.Split();// нормализация пробелов в ФИО
                                    lesson.CardProf = a[0] + " " + a[^1];
                                    break;
                            }
                        }

                        day.Lessons.Add(lesson);

                        string scout = string.Empty;//отслеживание границы между днями
                        do
                            scout += (char)reader.Read();
                        while (!(scout.Contains("td") || scout.Contains("th")));

                        if (scout.Contains("td")) continue;
                        else break;
                    }
                }

                if (buff.Contains("Полнодневные занятия"))
                {
                    reader.ReadLine();
                    reader.ReadLine();

                    do
                    {
                        buff = reader.ReadLine();

                        if (buff.Contains("<form id=\"print_form\" action=\"/tplan/print_preview.php\" target=\"_blank\" method=\"post\">"))//быстрофикс
                            break;

                        buff = CutHTML(buff);
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
                        var day = days.Find(x => x.Name == dayOfWeek);

                        var lesson = new Lesson();
                        reader.ReadLine();

                        buff = reader.ReadLine();
                        buff = CutHTML(buff);
                        lesson.CardName = buff;

                        buff = reader.ReadLine();
                        buff = CutHTML(buff);
                        lesson.CardDay = buff;


                        day.Lessons.Add(lesson);

                        buff = reader.ReadLine();
                    }
                    while (!buff.Contains("</table>"));
                    break;
                }
            }

            for (int i = 0; i < days.Count; i++)
            {
                if (days[i].Lessons.Count == 0)
                {
                    days[i].Lessons.Add(new Lesson() { CardName = "Выходной день", CardDay = "Еженедельно" });
                    continue;
                }

                if (!days[i].Lessons.Any(x => x.CardDay.Contains("Числ")) && !days[i].Lessons.Any(x => x.CardDay.Contains("Еже")))
                {
                    days[i].Lessons.Add(new Lesson() { CardName = "Выходной день", CardDay = "Числитель" });
                    continue;
                }

                if (!days[i].Lessons.Any(x => x.CardDay.Contains("Знам")) && !days[i].Lessons.Any(x => x.CardDay.Contains("Еже")))
                {
                    days[i].Lessons.Add(new Lesson() { CardName = "Выходной день", CardDay = "Знаменатель" });
                    continue;
                }
            }

            return days;
        }

        private static string? CutHTML(string? data, List<string> strToDelete = null)
        {
            strToDelete ??= new() { "table class=\"timetable\"", "colspan=6", "style=\"white-space:pre-wrap\"",
                                    "<", ">", "/", "br", "th", "tr", "td", "b", "colspan=\"2\"",
                                    "rowspan=\"1\"", "li", "class", "value=" };
            foreach (var str in strToDelete)
                data = data.Replace(str, string.Empty).Trim();
            return data;
        }
    }
    class ParseMADIException : Exception
    {
        public ParseMADIException(string message)
            : base(message) { }
    }
}

