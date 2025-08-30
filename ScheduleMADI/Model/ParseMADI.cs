using System.Collections.ObjectModel;

namespace ScheduleMADI
{
    public static class ParseMADI
    {
        public static Dictionary<string, string> id_groups = new();//словарь групп
        public static Dictionary<string, string> id_professors = new();

        private static TimeSpan TIMEOUT = new(0, 0, 10);
        public async static Task GetWeek(CancellationToken cancellationToken)
        {
            HttpClient httpClient = new();
            httpClient.Timeout = TIMEOUT;
            string responseString;

            var response = await httpClient.GetAsync("https://raspisanie.madi.ru/tplan/calendar.php", cancellationToken);
            responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            WeekMADI.Week = responseString switch
            {
                "{\"aaData\":[\"\\u0437\"]}" => "Знаменатель",
                "{\"aaData\":[\"\\u0447\"]}" => "Числитель",
                _ => "Получить день не удалось",
            };
            BufferedMADI.BufferedDay = new KeyValuePair<DateTime, string>(DateTime.Now.Date, WeekMADI.Week);
        }
        public async static Task GetGroups(CancellationToken cancellationToken)
        {
            HttpClient httpClient = new();

            httpClient.Timeout = TIMEOUT;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "step_no", "1" },

                { "task_id", "7" }
            });

            string responseString;

            var response = await httpClient.PostAsync("https://raspisanie.madi.ru/tplan/tasks/task3,7_fastview.php", content, cancellationToken);
            responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            using StringReader reader = new(responseString);

            List<string> list_buff = new();
            try
            {
                for (int i = 0; i < 11; i++)
                    reader.ReadLine();
                list_buff = reader.ReadLine().Trim().Split("<li").ToList();
                list_buff.RemoveAll(x => x == string.Empty);
                reader.Close();
                reader.Dispose();
            }
            catch (NullReferenceException)
            {
                return;
            }

            foreach (var buff in list_buff)
            {
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested(); ;

                var group_buff = CutHTML(buff).Trim().Split("\"").ToList();
                group_buff.RemoveAll(x => x == string.Empty);
                try//в полученном html могут быть айди группы без имен
                {
                    var id = group_buff[0].Trim();
                    var name = group_buff[1].Replace(" ", String.Empty);
                    if (!id_groups.ContainsKey(id)) // может быть плохо при повторном коннекте
                        id_groups.Add(id, name);
                }
                catch (ArgumentOutOfRangeException) { continue; }
            }
        }
        public async static Task GetProfessors(CancellationToken cancellationToken)
        {
            HttpClientHandler handler = new HttpClientHandler();
            //handler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) =>
            //{
            //    if (cert.Issuer.Equals("CN=R10, O=Let's Encrypt, C=US") && cert.Subject.Equals("CN=*.madi.ru"))
            //        return cert.GetCertHashString(HashAlgorithmName.SHA256)
            //                   .Equals("c94c0e2a3e30fe9105b79d84c2f709f7b0e22e52791d9b9cd10d1767f8975dd9".ToUpper());
            //    return errors == System.Net.Security.SslPolicyErrors.None;
            //};

            HttpClient httpClient = new(handler);

            httpClient.Timeout = TIMEOUT;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "step_no", "1" },

                { "task_id", "8" }
            });

            string responseString;

            var response = await httpClient.PostAsync("https://raspisanie.madi.ru/tplan/tasks/task8_prepview.php", content);
            responseString = await response.Content.ReadAsStringAsync();

            using StringReader reader = new(responseString);

            List<string> list_buff = new();
            try
            {
                for (int i = 0; i < 13; i++)
                    reader.ReadLine();
                list_buff = reader.ReadLine().Trim().Split("<option").ToList();
                list_buff.RemoveAll(x => x == string.Empty);
                reader.Close();
                reader.Dispose();
            }
            catch (NullReferenceException ex)
            {
                return;
            }

            foreach (var buff in list_buff)
            {

                var proff_buff = CutHTML(buff).Trim().Split("\"").ToList();
                proff_buff.RemoveAll(x => x == string.Empty);
                try//в полученном html могут быть айди группы без имен
                {
                    var id = proff_buff[0].Trim();
                    var name_buff = proff_buff[1].Split().ToList();
                    name_buff.RemoveAll(x => x == string.Empty);
                    var name = name_buff[0] + " " + name_buff[1];
                    if (!id_professors.ContainsKey(id) && id != "-1") // может быть плохо при повторном коннекте
                        id_professors.Add(id, name);
                }
                catch (ArgumentOutOfRangeException) { continue; }

            }
        }
        public async static Task<List<Day>> GetSchedule(KeyValuePair<string, string> id, CancellationToken cancellationToken)
        {
            FormUrlEncodedContent content;
            var date = SemesterCalculator(DateTime.Now);

            if (id_groups.ContainsKey(id.Key))
            {
                content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "tab", "7" },

                    { "gp_id", $"{id.Key}" },

                    {"tp_year", $"{date.year}" },

                    {"sem_no", $"{date.semester}" }
                });
            }
            else
            {
                content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "tab", "8" },

                    { "pr_id", $"{id.Key}" },

                    {"tp_year", $"{date.year}" },

                    {"sem_no", $"{date.semester}" }
                });
            }

            HttpClientHandler handler = new HttpClientHandler();
            //handler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) =>
            //{
            //    if (cert.Issuer.Equals("CN=R10, O=Let's Encrypt, C=US") && cert.Subject.Equals("CN=*.madi.ru"))
            //        return cert.GetCertHashString(HashAlgorithmName.SHA256)
            //                   .Equals("c94c0e2a3e30fe9105b79d84c2f709f7b0e22e52791d9b9cd10d1767f8975dd9".ToUpper());
            //    return errors == System.Net.Security.SslPolicyErrors.None;
            //};

            HttpClient httpClient = new(handler);

            httpClient.Timeout = TIMEOUT;

            string responseString;

            var response = await httpClient.PostAsync("https://raspisanie.madi.ru/tplan/tasks/tableFiller.php", content, cancellationToken);
            responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (responseString == "Извините, по данным атрибутам информация не найдена. Пожалуйста, укажите другие атрибуты")
                throw new ParseMADIException("На сайте сейчас нет данных об этой группе.");
            else if (responseString.Contains("Данная информация будет доступна с"))
                throw new ParseMADIException(responseString);
            else
            {
                var a = ParseHTML(responseString);

                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                BufferedMADI.BufferedSchedule = new KeyValuePair<string, string>(id.Key, responseString);
                return a;
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

            bool isProfessors = false;

            StringReader reader = new(html);

            foreach (var day in days)//это че такое вообще?
                if (day.Name != DayOfWeek.Sunday)
                    day.Lessons.Clear();//как это родилось?

            string buff;
            for (int row = 0; (buff = reader.ReadLine()) != null; row++)
            {
                if (row == 10 && buff.Contains("Преподаватель"))
                    isProfessors = true;

                if (buff.Contains("colspan=6") || buff.Contains("colspan=\"6\"") && !buff.Contains("Полнодневные занятия"))//отслеживание начала дня
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
                            if (!isProfessors)
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
                                        var a = buff.Split().ToList();// нормализация пробелов в ФИО
                                        a.RemoveAll(x => x == "");

                                        var full = "";
                                        foreach (var x in a)
                                            full += x + " ";
                                        lesson.CardProf = full;
                                        break;
                                }
                            else
                                switch (i)
                                {
                                    case 0:
                                        lesson.CardTime = buff;
                                        break;
                                    case 2:
                                        lesson.CardName = buff;
                                        break;
                                    case 3:
                                        lesson.CardType = buff;
                                        break;
                                    case 4:
                                        lesson.CardDay = buff;
                                        break;
                                    case 5:
                                        lesson.CardRoom = buff;
                                        break;
                                    case 1:
                                        lesson.CardProf = buff.Replace(" ", string.Empty);
                                        break;
                                }
                        }

                        var existingLesson = day.Lessons.SingleOrDefault(x =>
                        x.CardTime == lesson.CardTime && x.CardType == lesson.CardType &&
                        x.CardName == lesson.CardName && x.CardDay == lesson.CardDay && x.CardRoom == lesson.CardRoom,
                        null);

                        if (existingLesson != null)
                            existingLesson.CardProf += $", {lesson.CardProf}";
                        else
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

        public async static Task<List<Exam>> GetExamSchedule(KeyValuePair<string, string> id, CancellationToken cancellationToken)
        {
            FormUrlEncodedContent content;
            var date = SemesterCalculatorForExam(DateTime.Now);

            if (id_groups.ContainsKey(id.Key))
            {
                content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "tab", "3" },

                    { "gp_id", $"{id.Key}" },

                    {"tp_year", $"{date.year}" },

                    {"sem_no", $"{date.semester}" }
                });
            }
            else
            {
                content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "tab", "4" },

                    { "pr_id", $"{id.Key}" },

                    {"tp_year", $"{date.year}" },

                    {"sem_no", $"{date.semester}" }
                });
            }

            HttpClientHandler handler = new HttpClientHandler();
            //handler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) =>
            //{
            //    if (cert.Issuer.Equals("CN=R10, O=Let's Encrypt, C=US") && cert.Subject.Equals("CN=*.madi.ru"))
            //        return cert.GetCertHashString(HashAlgorithmName.SHA256)
            //                   .Equals("c94c0e2a3e30fe9105b79d84c2f709f7b0e22e52791d9b9cd10d1767f8975dd9".ToUpper());
            //    return errors == System.Net.Security.SslPolicyErrors.None;
            //};

            HttpClient httpClient = new(handler);

            httpClient.Timeout = TIMEOUT;

            string responseString;

            var response = await httpClient.PostAsync("https://raspisanie.madi.ru/tplan/tasks/tableFiller.php", content, cancellationToken);
            responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (responseString == "Извините, по данным атрибутам информация не найдена. Пожалуйста, укажите другие атрибуты")
                throw new ParseMADIException("На сайте сейчас нет данных об этой группе.");
            else if (responseString.Contains("Данная информация будет доступна с"))
                throw new ParseMADIException(responseString);
            else
            {
                var a = ParseExamHTML(responseString);

                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                BufferedMADI.BufferedExamSchedule = new KeyValuePair<string, string>(id.Key, responseString);
                return a;
            }
        }

        public async static Task<List<Exam>> GetExamScheduleFromHTML(string html)
        {
            return ParseExamHTML(html);
        }

        private static List<Exam> ParseExamHTML(string html)//парсинг html-таблицы экзаменов
        {
            var exams = new List<Exam>();
            bool forProfessors = html.Contains("Преподаватель:");
            StringReader reader = new(html);

            if (html == CutHTML(html))
            {
                exams.Add(new() { CardName = html });
            }
            else if (html.Contains("timetable") && (html.Contains("Информация по") || html.Contains("Данная информация")))
            {
                while (true)
                    if (reader.ReadLine().Contains("timetable"))
                        break;
                for (int i = 0; i < 6; i++)
                    reader.ReadLine();

                for (string buff; (buff = reader.ReadLine()) != "</tr>";)
                {
                    if (forProfessors)
                    {
                        var group = CutHTML(reader.ReadLine());
                        var datetime = CutHTML(reader.ReadLine());
                        var room = CutHTML(reader.ReadLine());
                        var name = CutHTML(reader.ReadLine());

                        exams.Add(new() { CardDateTime = datetime, CardName = name, CardProf = group, CardRoom = room });
                    }
                    else
                    {
                        var name = CutHTML(reader.ReadLine());
                        var datetime = CutHTML(reader.ReadLine());
                        var room = CutHTML(reader.ReadLine());
                        var group = CutHTML(reader.ReadLine());

                        exams.Add(new() { CardDateTime = datetime, CardName = name, CardProf = group, CardRoom = room });
                    }
                }

                var resp = reader.ReadLine();

                var infoStrings = resp.Split("</p>");

                for (int i = 0; i < infoStrings.Length - 1; i++)
                    exams.Add(new() { CardName = CutHTML(infoStrings[i]) });
            }
            else if (html.Contains("timetable") && !(html.Contains("Информация по") || html.Contains("Данная информация")))
            {
                while (true)
                    if (reader.ReadLine().Contains("timetable"))
                        break;
                for (int i = 0; i < 6; i++)
                    reader.ReadLine();

                for (string buff; (buff = reader.ReadLine()) != "</tr>";)
                {
                    if (forProfessors)
                    {
                        var group = CutHTML(reader.ReadLine());
                        var datetime = CutHTML(reader.ReadLine());
                        var room = CutHTML(reader.ReadLine());
                        var name = CutHTML(reader.ReadLine());

                        exams.Add(new() { CardDateTime = datetime, CardName = name, CardProf = group, CardRoom = room });
                    }
                    else
                    {
                        var name = CutHTML(reader.ReadLine());
                        var datetime = CutHTML(reader.ReadLine());
                        var room = CutHTML(reader.ReadLine());
                        var group = CutHTML(reader.ReadLine());

                        exams.Add(new() { CardDateTime = datetime, CardName = name, CardProf = group, CardRoom = room });
                    }
                }
            }
            else if (!html.Contains("timetable") && html.Contains("Информация"))
            {
                var resp = reader.ReadLine();

                var infoStrings = resp.Split("</p>");

                for (int i = 0; i < infoStrings.Length - 1; i++)
                    exams.Add(new() { CardName = CutHTML(infoStrings[i]) });
            }

            return exams;
        }

        private static string? CutHTML(string? data, List<string> strToDelete = null)
        {
            strToDelete ??= new() { "table class=\"timetable\"", "colspan=6", "colspan=\"6\"", "style=\"white-space:pre-wrap\"",
                                    "<", ">", "/", "br", "th", "tr", "td", "b", "colspan=\"2\"",
                                    "rowspan=\"1\"", "li", "class", "value=", "select", "option", "rowspan=\"8\"", "colspan=\"1\"",
                                    "talep","p"};
            foreach (var str in strToDelete)
                data = data.Replace(str, string.Empty).Trim();
            return data;
        }

        private static (int semester, int year) SemesterCalculator(DateTime date)
        {
            int semester, year;

            if (date.Date.Month >= 8 || (date.Date.Month == 1 && date.Date.Day < 2))
            {

                year = date.Year;
                semester = 1;

            }
            else
            {
                year = date.Year - 1;
                semester = 2;
            }

            if (date.Date.Month == 1 && date.Date.Day < 2)
                year -= 1;

            return (semester, year - 2000);
        }
        private static (int semester, int year) SemesterCalculatorForExam(DateTime date)
        {
            int semester, year;

            if (date.Date.Month >= 8 || (date.Date.Month == 1 && date.Date.Day < 30))
            {
                year = date.Year;
                semester = 1;
            }
            else
            {
                year = date.Year - 1;
                semester = 2;
            }

            if (date.Date.Month == 1 && date.Date.Day < 30)
                year -= 1;

            return (semester, year - 2000);
        }
    }
    class ParseMADIException : Exception
    {
        public ParseMADIException(string message)
            : base(message) { }
    }
}

