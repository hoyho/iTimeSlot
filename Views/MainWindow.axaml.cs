using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using SkiaSharp;

namespace iTimeSlot_avalonia.Views;

public partial class MainWindow : Window
{
    List<TimeSpan> _allTimeSlots = new();

    private DateTime iconLastUpdate = DateTime.MinValue;
    private object iconLock = new object();

    public MainWindow()
    {
        InitializeComponent();
        LoadTimeSlots();
    }

    private void LoadTimeSlots()
    {
        _allTimeSlots.Clear();

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
                _allTimeSlots = slots;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("setting.json not found, use default time slots");
            _allTimeSlots = iTimeSlot.Shared.DefaultConfig.SysTimeSlots;
        }

        pickerCurrentTimeSlot.ItemsSource = _allTimeSlots;
        pickerCurrentTimeSlot.SelectedIndex = 0;
        //pickerCurrentTimeSlot.SelectedIndex = 0; //todo: load from setting

        //todo slots.ItemsSource = _allTimeSlots;


    }

    public void OnStartClickHandler(object sender, RoutedEventArgs args)
    {
        // https://github.com/AvaloniaUI/Avalonia/issues/8076
        // //get and set tray icon
        // foreach (var i in TrayIcon.GetIcons(Application.Current).First().Menu.Items)
        // {
        //     Console.WriteLine("menu item:"+ i.GetValue(NativeMenu.MenuProperty));
        // }
        // TrayIcon.GetIcons(Application.Current).First().Menu.Items.Add(new NativeMenuItem("dynamic"));

        var tm = iTimeSlot.Shared.Global.MyTimer;
        var selected = _allTimeSlots[pickerCurrentTimeSlot.SelectedIndex]; //todo use safe 
        tm.Init(DateTime.Now, selected, this.ProgressTo, this.DisplayTimeupAlert);
        //update to full before start which will be reset to 0
        //await progressBar.ProgressTo(1, 0, Easing.Default);
        progressBar.IsVisible = true;

        tm.Start();
        pickerCurrentTimeSlot.IsEnabled = false;
        StartBtn.IsEnabled = false;
    }

    public void ProgressTo(double leftPercentage)
    {

        Dispatcher.UIThread.Invoke(() =>
        {
            // progressBar.Value = val;
            progressBar.SetValue(ProgressBar.ValueProperty, leftPercentage);
            Console.WriteLine("progress: " + (int)  leftPercentage);
            var almostDone = (int) leftPercentage < 10;
       

            if (DateTime.Now - iconLastUpdate < TimeSpan.FromSeconds(2) && !almostDone )
            {
                return;
            }


            lock (iconLock)
            {
                var bm = Foundation.Imaging.GenerateTrayIcon(leftPercentage);
                using (var stream = new MemoryStream())
                {
                    bm.Encode(stream, SKEncodedImageFormat.Png, 100); // 将 SKBitmap 对象保存为 PNG 格式到流中
                                                                      // 将流的位置重置到起始位置，以便后续读取
                    stream.Position = 0;
                    WindowIcon icon = new WindowIcon(stream);
                    TrayIcon.GetIcons(Application.Current).First().Icon = icon;
                }
            }

            iconLastUpdate = DateTime.Now;
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

        lock (iconLock)
        {
            Foundation.Imaging.ResetTrayIcon();
        }

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
                     MaxWidth = 600,
                     MaxHeight = 900,
                     SizeToContent = SizeToContent.WidthAndHeight,
                     ShowInCenter = true,
                     Topmost = false,
                 });
             var result = await box.ShowAsync();
             var args = new RoutedEventArgs();
             if (result == "Restart")
             {
                Console.WriteLine("Restart");
                 if (iTimeSlot.Shared.Global.MyTimer.IsStarted())
                 {
                     return;
                 }
                 OnCancelClickHandler(this, args);
                 OnStartClickHandler(this, args);
             }
             else if (result == "Ok")
             {
                Console.WriteLine("Ok");
                 if (iTimeSlot.Shared.Global.MyTimer.IsStarted())
                 {
                     return;
                 }
                 OnCancelClickHandler(this, args);
             }
         }));

    }

}