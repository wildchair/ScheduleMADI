using ScheduleMADI.Interfaces;

namespace ScheduleMADI;

public partial class ExamPage : ContentPage
{
    public ExamPage(IExamPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}