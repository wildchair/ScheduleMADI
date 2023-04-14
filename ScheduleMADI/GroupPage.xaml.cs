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

    private string button_text = "���������";
    private bool button_is_enabled = false;
    public GroupPage()
    {
        InitializeComponent();
    }


    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ParseMADI.id_groups.Any(x => x.Value.ToLower().Equals(e.NewTextValue.ToLower())))
        {
            Button_text = "���������";
            Button_is_enabled = true;
            //------------------//
            entry.IsEnabled = false;//��� ����������, � ������ �� ������� ����� ������������� ����� ������ �� enable/disable
            entry.IsEnabled = true;
            //------------------//
        }
        else
        {
            Button_is_enabled = false;
            Button_text = "���������";
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        IdMADI.Id = ParseMADI.id_groups.Where(x => x.Value.ToLower().Equals(entry.Text.ToLower())).Single();
        Button_text = "���������";
        try
        {
            await ParseMADI.GetShedule(IdMADI.Id.Key);
        }
        catch
        {
            await DisplayAlert("������", "�� ������� ��������� ���������� :(\n��������, ��� ���������� � ����������.", "��");
            return;
        }
    }
}
