namespace ScheduleMADI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		if(Preferences.Default.ContainsKey("id_group"))
            IdMADI.Id = IdMADI.LoadSavedID();
        MainPage = new AppShell();
	}

}
