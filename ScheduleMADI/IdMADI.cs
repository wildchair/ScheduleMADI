using System.ComponentModel;

namespace ScheduleMADI
{
    internal static class IdMADI
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
            }
        }

        public static KeyValuePair<string, string> LoadSavedID()
        {
         return new KeyValuePair<string, string>(Preferences.Default.Get("id_group", "8810"), 
                                                 Preferences.Default.Get("name_group", "1бАС1"));
        }
    }
}

