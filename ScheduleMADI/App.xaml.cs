namespace ScheduleMADI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        BufferedMADI.Restore();

        MainPage = new AppShell();
	}

}
