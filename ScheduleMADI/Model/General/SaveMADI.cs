using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleMADI
{
    //надо сохранять еще и день недели
    public class SaveMADI
    {
        private static KeyValuePair<DateTime, string> scheduleHTML;
        public static KeyValuePair<DateTime, string> ScheduleHTML
        {
            get => scheduleHTML;
            set
            {
                scheduleHTML = value;
                Preferences.Default.Set("last_date_load", value.Key);
                Preferences.Default.Set("scheduleHTML", value.Value);
                OnPropertyChanged(nameof(scheduleHTML));
            }
        }

        public static KeyValuePair<DateTime, string> LoadSavedSchedule()//загрузка сохраненного html
        {
            return new KeyValuePair<DateTime, string>(Preferences.Default.Get("last_date_load", DateTime.Now),//данные по-умолчанию никогда не
                                                    Preferences.Default.Get("scheduleHTML", "default"));//возникнут, тк вызовы функции 
        }                                                                                               //сопровождаются проверкой наличия

        public static event PropertyChangedEventHandler PropertyChanged;
        private static void OnPropertyChanged([CallerMemberName] string name = null)//уведомление об изменении
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
        }
    }
}
