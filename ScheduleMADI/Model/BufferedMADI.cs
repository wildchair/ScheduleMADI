using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleMADI
{
    internal static class BufferedMADI
    {
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
                Preferences.Default.Set("buff_sche", value.Value);
                Preferences.Default.Set("buff_id", value.Key);
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
                Preferences.Default.Set("buff_exam_sche", value.Value);
                Preferences.Default.Set("buff_id", value.Key);
            }
        }

        public static KeyValuePair<string, string> LoadSavedID()//загрузка сохраненного idшника
        {
            return new KeyValuePair<string, string>(Preferences.Default.Get("id_group", "8810"),//данные по-умолчанию никогда не
                                                    Preferences.Default.Get("name_group", "1бАС1"));//возникнут, тк вызовы функции
        }                                                                                           //сопровождаются проверкой наличия

        public static KeyValuePair<string, string> LoadBufferedSchedule()
        {
            return new KeyValuePair<string, string>(Preferences.Default.Get("buff_id", String.Empty),
                                                    Preferences.Default.Get("buff_sche", String.Empty));
        }

        public static KeyValuePair<string, string> LoadBufferedExamSchedule()
        {
            return new KeyValuePair<string, string>(Preferences.Default.Get("buff_id", String.Empty),
                                                    Preferences.Default.Get("buff_exam_sche", String.Empty));
        }

        public static KeyValuePair<DateTime, string> LoadBufferedDay()
        {
            return new KeyValuePair<DateTime, string>(Preferences.Default.Get("day", DateTime.MinValue),
                                                    Preferences.Default.Get("typeofWeek", String.Empty));
        }

        public static event PropertyChangedEventHandler PropertyChanged;
        private static void OnPropertyChanged([CallerMemberName] string name = null)//уведомление об изменении ID
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
        }
    }
}

