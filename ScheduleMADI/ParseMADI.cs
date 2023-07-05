using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace ScheduleMADI
{
    public static class ParseMADI
    {
        public static bool reloaded = false;

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
                var group_buff = ParseDataHTML(buff).Trim().Split("\"").ToList();
                group_buff.RemoveAll(x => x == string.Empty);
                var id = group_buff[0].Trim();
                var name = group_buff[1].Replace(" ", String.Empty);
                if (!id_groups.ContainsKey(id))
                    id_groups.Add(id, name);
            }



            //var str = responseString.Split(' ').ToList();
            //str.RemoveAll(str => !str.Contains("value"));
            //char[] charsToTrim = { '<', '>', '/', 'l', 'i', '"' };
            //for (int i = 0; i < str.Count; i++)
            //{
            //    str[i] = str[i].Remove(0, 7).Replace('<', ' ').Replace('>', ' ').Replace('/', ' ').Replace('l', ' ').Replace('i', ' ').Replace('"', ' ');
            //}

            //foreach (var item in str)
            //{
            //    var buff = item.Split(" ").ToList();
            //    buff.RemoveAll(str => str.Length == 0);

            //    string id = buff[0], name = buff[1];
            //    if (!id_groups.ContainsKey(id))
            //        id_groups.Add(id, name);
            //}
        }
        public async static Task<List<Day>> GetShedule(string gp_id)
        {
            List<Day> days = new()
            {
                new Day(DayOfWeek.Monday), new Day(DayOfWeek.Tuesday),
                new Day(DayOfWeek.Wednesday), new Day(DayOfWeek.Thursday),
                new Day(DayOfWeek.Friday), new Day(DayOfWeek.Saturday),
                new Day(DayOfWeek.Sunday) {Lessons = new ObservableCollection<Lesson>()
                { new Lesson { CardName = "Выходной день", CardDay = "Еженедельно" } } }
            };


            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "tab", "7" },

                { "gp_id", $"{gp_id}" }
            });

            HttpClient httpClient = new();
            var response = await httpClient.PostAsync("https://raspisanie.madi.ru/tplan/tasks/tableFiller.php", content);
            var responseString = await response.Content.ReadAsStringAsync();

            StringReader reader = new(responseString);

            reloaded = true;

            foreach (var day in days)
                if (day.Name != DayOfWeek.Sunday)
                    day.Lessons.Clear();

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
                    var day = days.Find(x => x.Name == dayOfWeek);

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
                                    var a = buff.Split();
                                    lesson.CardProf = a[0] + " " + a[^1];
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
                        var day = days.Find(x => x.Name == dayOfWeek);

                        var lesson = new Lesson();
                        reader.ReadLine();

                        buff = reader.ReadLine();
                        buff = ParseDataHTML(buff);
                        lesson.CardName = buff;

                        buff = reader.ReadLine();
                        buff = ParseDataHTML(buff);
                        lesson.CardDay = buff;


                        day.Lessons.Add(lesson);

                        buff = reader.ReadLine();
                    }
                    while (!buff.Contains("</table>"));
                    break;
                }
            }

            return days;
        }
        private static string? ParseDataHTML(string? data, List<string> strToDelete = null)
        {
            strToDelete ??= new() { "table class=\"timetable\"", "colspan=6", "style=\"white-space:pre-wrap\"",
                                    "<", ">", "/", "br", "th", "tr", "td", "b", "colspan=\"2\"",
                                    "rowspan=\"1\"", "li", "class", "value=" };
            foreach (var str in strToDelete)
                data = data.Replace(str, string.Empty).Trim();
            return data;
        }
    }
}

