namespace ScheduleMADI
{

    public static class Groups
    {
        public static Dictionary<string, string> id_group = new();
        public async static Task GetList()
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
                id_group.Add(id, name);
            }
            return;
        }
        public async static Task GetShedule()
        {
            HttpClient httpClient = new HttpClient();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "tab", "7" },

                { "gp_id", "8716" }
            });

            var response = await httpClient.PostAsync("https://www.madi.ru/tplan/tasks/tableFiller.php", content);

            var responseString = await response.Content.ReadAsStringAsync();
            //webView.Source = new HtmlWebViewSource
            //{
            //    Html = responseString
            //};
            return;
        }
    }
}

