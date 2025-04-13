using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using ScheduleCore.Models;
using ScheduleCore.Models.Madi;
using System.Collections.ObjectModel;

namespace ScheduleCore.MadiSiteApiHelpers.Parsers
{
    public class HtmlHardcoreParser : IParser
    {
        public TypeOfWeek ParseWeek(string json)
        {
            switch (json)
            {
                case "{\"aaData\":[\"\\u0437\"]}":
                    return TypeOfWeek.Denominator;
                case "{\"aaData\":[\"\\u0447\"]}":
                    return TypeOfWeek.Numerator;
                default:
                    throw new FormatException("Couldn't parse the day of the week.");
            }
        }

        public MadiEntityRegistry ParseGroups(string html)
        {
            var groups = new Dictionary<int, string>();

            using StringReader reader = new(html);

            for (int i = 0; i < 11; i++)
                reader.ReadLine();
            var list_buff = reader.ReadLine().Split("</li>").ToList();
            list_buff.RemoveAll(x => x == string.Empty);

            foreach (var buff in list_buff)
            {
                var group_buff = CutHTML(buff).Trim().Split("\"").ToList();
                group_buff.RemoveAll(x => x == string.Empty);
                try//в полученном html могут быть айди группы без имен
                {
                    var id = int.Parse(group_buff[0].Trim());
                    var name = group_buff[1].Replace(" ", string.Empty);
                    groups.Add(id, name);
                }
                catch (ArgumentOutOfRangeException) { continue; }
            }

            return new() { Registry = groups };
        }

        public MadiEntityRegistry ParseProfessors(string html)
        {
            var professors = new Dictionary<int, string>();

            using StringReader reader = new(html);

            for (int i = 0; i < 13; i++)
                reader.ReadLine();
            var list_buff = reader.ReadLine().Split("<option").ToList();
            list_buff.RemoveAll(x => x == string.Empty);

            foreach (var buff in list_buff)
            {
                if (buff.Contains("<select id=\"prepChoose\" name=\"pr_id\" onchange=\"loadTable(8,2)\">"))
                    continue;
                var proff_buff = CutHTML(buff).Trim().Split("\"").ToList();
                proff_buff.RemoveAll(x => x == string.Empty);
                try//в полученном html могут быть айди группы без имен
                {
                    var id = int.Parse(proff_buff[0].Trim());
                    var name_buff = proff_buff[1].Split().ToList();
                    name_buff.RemoveAll(x => x == string.Empty);
                    var name = name_buff[0] + " " + name_buff[1];
                    if (id != -1)
                        professors.Add(id, name);
                }
                catch (ArgumentOutOfRangeException) { continue; }
            }

            return new() { Registry = professors };
        }

        public List<Day> ParseSchedule(string html)
        {
            List<Day> days = new()
            {
                new Day(DayOfWeek.Monday), new Day(DayOfWeek.Tuesday),
                new Day(DayOfWeek.Wednesday), new Day(DayOfWeek.Thursday),
                new Day(DayOfWeek.Friday), new Day(DayOfWeek.Saturday),
                new Day(DayOfWeek.Sunday) {Lessons = new ObservableCollection<Class>()
                { new Class { Name = "Выходной день", TypeOfWeek = "Еженедельно" } } }
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
                        var lesson = new Class();
                        for (int i = 0; i < 6; i++)//парсинг данных Lesson-а
                        {
                            buff = reader.ReadLine();
                            buff = CutHTML(buff);
                            if (!isProfessors)
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
                                        lesson.TypeOfWeek = buff;
                                        break;
                                    case 4:
                                        lesson.Classroom = buff;
                                        break;
                                    case 5:
                                        var a = buff.Split().ToList();// нормализация пробелов в ФИО
                                        a.RemoveAll(x => x == "");

                                        var full = "";
                                        foreach (var x in a)
                                            full += x + " ";
                                        lesson.Visitors = full;
                                        break;
                                }
                            else
                                switch (i)
                                {
                                    case 0:
                                        lesson.Time = buff;
                                        break;
                                    case 2:
                                        lesson.Name = buff;
                                        break;
                                    case 3:
                                        lesson.Type = buff;
                                        break;
                                    case 4:
                                        lesson.TypeOfWeek = buff;
                                        break;
                                    case 5:
                                        lesson.Classroom = buff;
                                        break;
                                    case 1:
                                        lesson.Visitors = buff.Replace(" ", string.Empty);
                                        break;
                                }
                        }

                        var existingLesson = day.Lessons.SingleOrDefault(x =>
                        x.Time == lesson.Time && x.Type == lesson.Type &&
                        x.Name == lesson.Name && x.TypeOfWeek == lesson.TypeOfWeek && x.Classroom == lesson.Classroom,
                        null);

                        if (existingLesson != null)
                            existingLesson.Visitors += $", {lesson.Visitors}";
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

                        var lesson = new Class();
                        reader.ReadLine();

                        buff = reader.ReadLine();
                        buff = CutHTML(buff);
                        lesson.Name = buff;

                        buff = reader.ReadLine();
                        buff = CutHTML(buff);
                        lesson.TypeOfWeek = buff;


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
                    days[i].Lessons.Add(new Class() { Name = "Выходной день", TypeOfWeek = "Еженедельно" });
                    continue;
                }

                if (!days[i].Lessons.Any(x => x.TypeOfWeek.Contains("Числ")) && !days[i].Lessons.Any(x => x.TypeOfWeek.Contains("Еже")))
                {
                    days[i].Lessons.Add(new Class() { Name = "Выходной день", TypeOfWeek = "Числитель" });
                    continue;
                }

                if (!days[i].Lessons.Any(x => x.TypeOfWeek.Contains("Знам")) && !days[i].Lessons.Any(x => x.TypeOfWeek.Contains("Еже")))
                {
                    days[i].Lessons.Add(new Class() { Name = "Выходной день", TypeOfWeek = "Знаменатель" });
                    continue;
                }
            }

            return days;
        }

        public List<Exam> ParseExamSchedule(string html)
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
        private string? CutHTML(string? data, List<string> strToDelete = null)
        {
            strToDelete ??= new() { "table class=\"timetable\"", "colspan=6", "colspan=\"6\"", "style=\"white-space:pre-wrap\"",
                                    "<", ">", "/", "br", "th", "tr", "td", "b", "colspan=\"2\"",
                                    "rowspan=\"1\"", "li", "class", "value=", "select", "option", "rowspan=\"8\"", "colspan=\"1\"",
                                    "talep","p"};
            foreach (var str in strToDelete)
                data = data.Replace(str, string.Empty).Trim();
            return data;
        }

    }
}