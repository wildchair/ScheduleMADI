using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleMADI
{
    internal static class BufferedMADI
    {
        private const string ScheduleKey = "schedule_cache";
        private const string ExamsKey = "exam_cache";

        private static KeyValuePair<string, string> id;
        public static KeyValuePair<string, string> Id
        {
            get => id;
            set
            {
                id = value;
                Preferences.Default.Set("id_group", value.Key);
                Preferences.Default.Set("name_group", value.Value);
                OnPropertyChanged(nameof(Id));
            }
        }

        private static KeyValuePair<string, string> bufferedSchedule;
        public static KeyValuePair<string, string> BufferedSchedule
        {
            get => bufferedSchedule;
            set
            {
                bufferedSchedule = value;
                Preferences.Default.Set(ScheduleKey, EncodeCache(value));
            }
        }

        private static KeyValuePair<DateTime, string> bufferedDay;
        public static KeyValuePair<DateTime, string> BufferedDay
        {
            get => bufferedDay;
            set
            {
                bufferedDay = value;
                Preferences.Default.Set("day", value.Key);
                Preferences.Default.Set("typeofWeek", value.Value);
            }
        }

        private static KeyValuePair<string, string> bufferedExamSchedule;
        public static KeyValuePair<string, string> BufferedExamSchedule
        {
            get => bufferedExamSchedule;
            set
            {
                bufferedExamSchedule = value;
                Preferences.Default.Set(ExamsKey, EncodeCache(value));
            }
        }

        public static void Restore()
        {
            if (Preferences.Default.ContainsKey("id_group"))
                id = new(Preferences.Default.Get("id_group", string.Empty),
                         Preferences.Default.Get("name_group", string.Empty));

            bufferedDay = new(Preferences.Default.Get("day", DateTime.MinValue),
                              Preferences.Default.Get<string>("typeofWeek", null));
            bufferedSchedule = DecodeCache(Preferences.Default.Get(ScheduleKey, string.Empty));
            bufferedExamSchedule = DecodeCache(Preferences.Default.Get(ExamsKey, string.Empty));
        }

        // Идентификатор и HTML записываются вместе, чтобы не связать старые данные с новой группой.
        private static string EncodeCache(KeyValuePair<string, string> value) => value.Key + "\n" + value.Value;

        private static KeyValuePair<string, string> DecodeCache(string entry)
        {
            var separator = entry.IndexOf('\n');
            return separator > 0 && separator < entry.Length - 1
                ? new(entry[..separator], entry[(separator + 1)..])
                : default;
        }

        public static event PropertyChangedEventHandler PropertyChanged;
        private static void OnPropertyChanged([CallerMemberName] string name = null)//уведомление об изменении ID
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
        }
    }
}

