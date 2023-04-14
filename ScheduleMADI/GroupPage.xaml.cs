namespace ScheduleMADI;
public partial class GroupPage : ContentPage
{
    public string Button_text
    {
        get => button_text;
        set
        {
            if (value != button_text)
            {
                button_text = value;
                OnPropertyChanged();
            }
        }
    }
    public bool Button_is_enabled
    {
        get => button_is_enabled;
        set
        {
            if (button_is_enabled != value)
            {
                button_is_enabled = value;
                OnPropertyChanged();
            }
        }
    }

    private string button_text = "Сохранить";
    private bool button_is_enabled = false;
    public GroupPage()
    {
        InitializeComponent();
    }


    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ParseMADI.id_groups.Any(x => x.Value.Equals(e.NewTextValue)))
        {
            //Button_text = "Сохранить";
            Button_is_enabled = true;
        }
        else
        {
            Button_is_enabled = false;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        ParseMADI.ID = ParseMADI.id_groups.Where(x => x.Value.Equals(entry.Text)).Single();
        await ParseMADI.GetShedule(ParseMADI.ID.Key);
    }
}
