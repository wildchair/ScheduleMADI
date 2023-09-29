namespace ScheduleMADI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        if (Preferences.Default.ContainsKey("id_group"))
            IdMADI.Id = IdMADI.LoadSavedID();
		if (Preferences.Default.ContainsKey("buff_sche"))
			IdMADI.BufferedSchedule = IdMADI.LoadBufferedSchedule();
		if(Preferences.Default.ContainsKey("day"))
			IdMADI.BufferedDay = IdMADI.LoadBufferedDay();
        MainPage = new AppShell();
	}

}
