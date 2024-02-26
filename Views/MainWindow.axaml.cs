using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using iTimeSlot.Foundation;
using MsBox.Avalonia;
using MsBox.Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using MsBox.Avalonia.ViewModels;
using MsBox.Avalonia.Windows;

namespace iTimeSlot.Views;

public partial class MainWindow : Window
{
    List<TimeSpan> _allTimeSlots = new();

    private DateTime _iconLastUpdate = DateTime.MinValue;
    private readonly object _iconLock = new object();
    private readonly TrayHelper _trayHelper;

    public MainWindow()
    {
        InitializeComponent();
        LoadTimeSlots();
        _trayHelper = new TrayHelper();
    }

    private void LoadTimeSlots()
    {
        _allTimeSlots.Clear();

        Debug.WriteLine("LoadTimeSlots");

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
            var almostDone = (int)leftPercentage < 10;


            if (DateTime.Now - _iconLastUpdate < TimeSpan.FromSeconds(2) && !almostDone)
            {
                return;
            }


            lock (_iconLock)
            {
                _trayHelper.SetPercentageTrayIcon(leftPercentage);
            }

            _iconLastUpdate = DateTime.Now;
        });
    }

    public void OnCancelClickHandler(object sender, RoutedEventArgs args)
    {
        iTimeSlot.Shared.Global.MyTimer.Stop();
        //this.ProgressTo(0);
        progressBar.IsVisible = false;
        StartBtn.IsEnabled = true;
        pickerCurrentTimeSlot.IsEnabled = true;

        lock (_iconLock)
        {
            //potential memory leak here
            //_trayHelper.ResetTrayIcon();
        }

    }

    private async void DisplayTimeupAlert()
    {
        await Dispatcher.UIThread.Invoke(async () =>
         {



            //  var box = MessageBoxManager.GetMessageBoxCustom(
            //      new MessageBoxCustomParams
            //      {
            //          ButtonDefinitions = new List<ButtonDefinition>
            //          {
            //             new ButtonDefinition { Name = "Ok", },
            //             new ButtonDefinition { Name = "Restart", },
            //          },
            //          ContentTitle = "Timer done",
            //          ContentMessage = "Have a break",
            //          Icon = MsBox.Avalonia.Enums.Icon.Info,
            //          WindowStartupLocation = WindowStartupLocation.CenterOwner,
            //          CanResize = false,
            //          MaxWidth = 600,
            //          MaxHeight = 900,
            //          SizeToContent = SizeToContent.WidthAndHeight,
            //          ShowInCenter = true,
            //          Topmost = false,
            //      });
            //  var result = await box.ShowWindowDialogAsync(this);
            var box = new AlertBox();
            var result = await box.ShowAlertAsync();
             var args = new RoutedEventArgs();
             if (result == "Restart")
             {
                 if (iTimeSlot.Shared.Global.MyTimer.IsStarted())
                 {
                     return;
                 }
                 OnCancelClickHandler(this, args);
                 OnStartClickHandler(this, args);
             }
             else if (result == "Ok")
             {
                 if (iTimeSlot.Shared.Global.MyTimer.IsStarted())
                 {
                     return;
                 }
                 OnCancelClickHandler(this, args);
             }
             //potential memory leak here unless the box is closed rather than ok or restart button clicked
             box.Close();
         });

    }


    internal class AlertBox
    {
        readonly MsBoxWindow window;
        readonly MsBoxCustomView msBoxCustomView;
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

        public AlertBox()
        {
            var msBoxCustomViewModel = new MsBoxCustomViewModel(new MessageBoxCustomParams
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

            this.msBoxCustomView = new MsBoxCustomView
            {
                DataContext = msBoxCustomViewModel
            };
            msBoxCustomViewModel.SetFullApi(msBoxCustomView);

            this.window = new MsBoxWindow
            {
                Content = msBoxCustomView,
                DataContext = msBoxCustomViewModel
            };
            window.Closed += msBoxCustomView.CloseWindow;
            
            msBoxCustomView.SetCloseAction(() =>
            {
                tcs.TrySetResult(msBoxCustomView.GetButtonResult());
                window.Close();
            });

        }


        public Task<string> ShowAlertAsync()
        {
            window.Show();
            return tcs.Task;
        }

        public void Close()
        {
            msBoxCustomView.CloseWindow(null, null);
            window.Close();
        }

    }

}