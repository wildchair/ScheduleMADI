﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ScheduleMADI;
public partial class MainPage : ContentPage
{
    MainPageVM mainPageVM = new();
    public MainPage()
    {
        InitializeComponent();
        BindingContext = mainPageVM;
        //BindingContext = new MainPageVM();
    }

    private async void Mainpage_Appearing(object sender, EventArgs e)
    {
        if (!Preferences.Default.ContainsKey("tap_instr"))
        {
            await DisplayAlert("Подсказка", "Двойное касание правой/левой части экрана позволяет переключаться между днями." +
                "Конкретную дату можно выбрать в календаре.", "Ок");
            Preferences.Default.Set("tap_instr", 1);
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make("Наблюдаются сбои в работе сайта.", ToastDuration.Long);
        await toast.Show(cancellationTokenSource.Token);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Vibration.Vibrate(100);
        if (Window.Width / 2 > e.GetPosition(null).Value.X && mainPageVM.withoutCarouselVM.DatepickerDate > mainPageVM.MinDate)
            mainPageVM.withoutCarouselVM.DatepickerDate = mainPageVM.withoutCarouselVM.DatepickerDate.AddDays(-1);
        else if (Window.Width / 2 < e.GetPosition(null).Value.X && mainPageVM.withoutCarouselVM.DatepickerDate < mainPageVM.MaxDate)
            mainPageVM.withoutCarouselVM.DatepickerDate = mainPageVM.withoutCarouselVM.DatepickerDate.AddDays(1);
    }

}