using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using ScheduleMADI.Interfaces;
using ScheduleMADI.Test;
using ScheduleMADI.ViewModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ScheduleMADI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<ISettingsPageViewModel, TestSettingsPageViewModel>();
		builder.Services.AddSingleton<IScheduleTargetProvider, TestScheduleTargetProvider>();
		builder.Services.AddSingleton<IExamPageViewModel, TestExamPageViewModel>();
		builder.Services.AddSingleton<IMainPageViewModel, TestMainPageViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

}
