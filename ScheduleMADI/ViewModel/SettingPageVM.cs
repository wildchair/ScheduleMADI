using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ScheduleMADI
{
    public class SettingPageVM : INotifyPropertyChanged
    {
        public string VersionApp { get => versionApp; set => versionApp = value; }
        private string versionApp = AppInfo.Current.VersionString;

        public string EntryText
        {
            get => entryText;
            set
            {
                if (entryText != value)
                {
                    entryText = value;
                    OnPropertyChanged();
                }
            }
        }
        private string entryText;

        public bool Entry_is_enabled
        {
            get => entry_is_enabled;
            set
            {
                if (entry_is_enabled != value)
                {
                    entry_is_enabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool entry_is_enabled = true;

        public string ButtonText
        {
            get => buttonText;
            set
            {
                if (buttonText != value)
                {
                    buttonText = value;
                    OnPropertyChanged();
                }
            }
        }
        private string buttonText = "Сохранить";

        public ICommand SaveGroup { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingPageVM()
        {
            //ParseMADI.PropertyChanged += OnLoadingChanged;
            SaveGroup = new Command(() =>
            {
                BufferedMADI.Id = ParseMADI.id_groups.Where(x => x.Value.ToLower().Equals(EntryText.ToLower())).Single();
                ((Command)SaveGroup).ChangeCanExecute();

                Entry_is_enabled = false;//сброс экранной клавиатуры
                Entry_is_enabled = true;
            },
            () =>
            {
                if (EntryText == null)//первый старт без сохраненной группы
                    return false;

                if (BufferedMADI.Id.Value != null && EntryText.ToLower() == BufferedMADI.Id.Value.ToLower())//если такая группа уже сохранена
                {
                    ButtonText = "Сохранено";
                    return false;
                }
                ButtonText = "Сохранить";

                return ParseMADI.id_groups.Any(x => x.Value.ToLower().Equals(EntryText.ToLower()))/* && !ParseMADI.Loading*/;// проверка существования введенной группы
            });

            if (BufferedMADI.Id.Value != null)
                EntryText = BufferedMADI.Id.Value;
        }

        //private void OnLoadingChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    ((Command)SaveGroup).ChangeCanExecute();
        //}
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            ((Command)SaveGroup).ChangeCanExecute();
        }
    }
}
