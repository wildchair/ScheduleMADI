namespace ScheduleMADI;

public partial class ExamPage : ContentPage
{
	public ExamPage()
	{
		InitializeComponent();
		BindingContext = new ExamPageVM();
	}
}