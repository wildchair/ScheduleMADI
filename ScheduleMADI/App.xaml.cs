namespace ScheduleMADI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        if (Preferences.Default.ContainsKey("id_group"))
            BufferedMADI.Id = BufferedMADI.LoadSavedID();
		if (Preferences.Default.ContainsKey("buff_sche") && Preferences.Default.ContainsKey("day"))
		{
			BufferedMADI.BufferedSchedule = BufferedMADI.LoadBufferedSchedule();//перезаписывается в память, надо починить
			BufferedMADI.BufferedDay = BufferedMADI.LoadBufferedDay();
		}
        if (Preferences.Default.ContainsKey("buff_exam_sche"))
        {
            BufferedMADI.BufferedExamSchedule = BufferedMADI.LoadBufferedExamSchedule();//перезаписывается в память, надо починить
        }

        MainPage = new AppShell();
	}

}
