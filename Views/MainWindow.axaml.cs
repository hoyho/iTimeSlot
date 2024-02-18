using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace iTimeSlot_avalonia.Views;

public partial class MainWindow : Window
{
    List<TimeSpan> allTimeSlots = new();

    static bool isSetup = false;
    public MainWindow()
    {
        InitializeComponent();
        LoadTimeSlots();
    }
    private void LoadTimeSlots()
    {
        allTimeSlots.Clear();

        Console.WriteLine("LoadTimeSlots");

        string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string fileName = Path.Combine(dir, "time_slots.json");

        if (File.Exists(fileName))
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                Console.WriteLine(jsonString);
                List<TimeSpan> slots = JsonSerializer.Deserialize<List<TimeSpan>>(jsonString);
                allTimeSlots = slots;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("setting.json not found, use default time slots");
            allTimeSlots = iTimeSlot.Shared.DefaultConfig.SysTimeSlots;
        }

        pickerCurrentTimeSlot.DataContext = allTimeSlots[0];
        pickerCurrentTimeSlot.SelectedTime = allTimeSlots[0];
        //pickerCurrentTimeSlot.SelectedIndex = 0; //todo: load from setting
    }

    public void OnStartClickHandler(object sender, RoutedEventArgs args)
    {

        var tm = iTimeSlot.Shared.Global.MyTimer;
        var selected = allTimeSlots[0];
        tm.Init(DateTime.Now, selected, this.ProgressTo, this.DisplayTimeupAlert);
        //update to full before start which will be reset to 0
        //await progressBar.ProgressTo(1, 0, Easing.Default);
        progressBar.IsVisible = true;

        tm.Start();
        pickerCurrentTimeSlot.IsEnabled = false;
        StartBtn.IsEnabled = false;
    }

    public void ProgressTo(double val)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            // progressBar.Value = val;
            progressBar.SetValue(ProgressBar.ValueProperty, val);
        });
    }

    public void OnCancelClickHandler(object sender, RoutedEventArgs args)
    {
        var tm = iTimeSlot.Shared.Global.MyTimer;
        tm.Stop();
        this.ProgressTo(0);
        progressBar.IsVisible = false;
        StartBtn.IsEnabled = true;
        pickerCurrentTimeSlot.IsEnabled = true;
    }

    private async void DisplayTimeupAlert()
    {
        await Dispatcher.UIThread.Invoke((async () =>
         {
             var box = MessageBoxManager.GetMessageBoxCustom(
                 new MessageBoxCustomParams
                 {
                     ButtonDefinitions = new List<ButtonDefinition>
                     {
                        new ButtonDefinition { Name = "Ok", },
                        new ButtonDefinition { Name = "Restart", },
                     },
                     ContentTitle = "Timer done",
                     ContentMessage = "Have a break",
                     Icon = MsBox.Avalonia.Enums.Icon.Info,
                     WindowStartupLocation = WindowStartupLocation.CenterOwner,
                     CanResize = false,
                     MaxWidth = 500,
                     MaxHeight = 800,
                     SizeToContent = SizeToContent.WidthAndHeight,
                     ShowInCenter = true,
                     Topmost = false,
                 });
             var result = await box.ShowAsync();
         }));

    }

}