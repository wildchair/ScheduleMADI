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

        public bool Coll_is_visible
        {
            get => coll_is_visible;
            set
            {
                if (coll_is_visible != value)
                {
                    coll_is_visible = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool coll_is_visible = false;

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
        public ICommand PerformSearch { get; set; }
        public ICommand ItemSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, string> SearchResults 
        {
            get => searchResults;
            set
            {
                searchResults = value;
                OnPropertyChanged();
            }
        }
        private Dictionary<string, string> searchResults;


        public SettingPageVM()
        {
            //SaveGroup = new Command(() =>
            //{
            //    BufferedMADI.Id = ParseMADI.id_groups.Where(x => x.Value.ToLower().Equals(EntryText.ToLower())).Single();
            //    ((Command)SaveGroup).ChangeCanExecute();

            //    Entry_is_enabled = false;//сброс экранной клавиатуры
            //    Entry_is_enabled = true;
            //},
            //() =>
            //{
            //    if (EntryText == null)//первый старт без сохраненной группы
            //        return false;

            //    if (BufferedMADI.Id.Value != null && EntryText.ToLower() == BufferedMADI.Id.Value.ToLower())//если такая группа уже сохранена
            //    {
            //        ButtonText = "Сохранено";
            //        return false;
            //    }
            //    ButtonText = "Сохранить";

            //    return ParseMADI.id_groups.Any(x => x.Value.ToLower().Equals(EntryText.ToLower()))/* && !ParseMADI.Loading*/;// проверка существования введенной группы
            //});
            
            PerformSearch = new Command(() =>
            {
                Coll_is_visible = true;
                SearchResults = ParseMADI.id_groups.Union(ParseMADI.id_professors).Where(x=>x.Value.ToLower().StartsWith(EntryText.ToLower())).ToDictionary(x => x.Key, x => x.Value);
            });

            ItemSelected = new Command<KeyValuePair<string, string>>((item) =>
            {
                BufferedMADI.Id = item;
                EntryText = item.Value;

                Entry_is_enabled = false;//сброс экранной клавиатуры
                Entry_is_enabled = true;
                Coll_is_visible = false;
            });

            if (BufferedMADI.Id.Value != null)
            {
                EntryText = BufferedMADI.Id.Value;
                Coll_is_visible = false;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //((Command)SaveGroup).ChangeCanExecute();
            if(name == nameof(EntryText))
                ((Command)PerformSearch).Execute(null);
        }
    }
}
