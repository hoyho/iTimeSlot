using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using iTimeSlot.Foundation;
using iTimeSlot.Models;
using iTimeSlot.Shared;
using iTimeSlot.Views;
using NetCoreAudio;
using ReactiveUI;

namespace iTimeSlot.ViewModels;

public partial class MainWindowViewModel : ObservableViewModelBase
{

    public MainWindowViewModel()
    {
        _trayHelper = new TrayHelper();
    }
    private ObservableCollection<TimeSlot> _slots;
    public ObservableCollection<TimeSlot> AllTimeSlots
    {
        get { return _slots; }
        set { SetProperty(ref _slots, value); }
    }

    private readonly object _mainLock = new object();
    private readonly TrayHelper _trayHelper;


    private decimal? _time2Add;

    [Range(1, 60, ErrorMessage = "value must be between {1} and {2}.")]
    public decimal? TimeToAdd
    {
        get { return _time2Add; }
        set { this.SetProperty(ref _time2Add, value); }
    }

    private int _indexOfTimeInWorkspace;

    public int IndexOfSelectedTimeInWorkspace
    {
        get { return _indexOfTimeInWorkspace; }
        set { this.SetProperty(ref _indexOfTimeInWorkspace, value); }
    }

    private int _indexOfTimeInSetting;
    public int IndexOfSelectedTimeInSetting
    {
        get { return _indexOfTimeInSetting; }
        set { this.SetProperty(ref _indexOfTimeInSetting, value); }
    }

    private double _progVal;
    public double ProgressValue
    {
        get { return _progVal; }
        set { this.SetProperty(ref _progVal, value); }
    }

    private bool _progressVisible;
    public bool ProgressVisible
    {
        get { return _progressVisible; }
        set { this.SetProperty(ref _progressVisible, value); }
    }

    private bool _isTimeSlotComboBoxEnabled = true;
    public bool IsTimeSlotComboBoxEnabled
    {
        get { return _isTimeSlotComboBoxEnabled; }
        set { this.SetProperty(ref _isTimeSlotComboBoxEnabled, value); }
    }
    private bool _isStartButtonEnabled = true;
    public bool IsStartButtonEnabled
    {
        get { return _isStartButtonEnabled; }
        set { this.SetProperty(ref _isStartButtonEnabled, value); }
    }


    private string _showProgressText;
    public string ShowProgressText
    {
        get { return _showProgressText; }
        set { this.SetProperty(ref _showProgressText, value); }
    }

    public WindowNotificationManager? SettingTabNotificationManager { get; set; }

    public bool CloseWithoutExit { get; set; }
    public bool PlaySound { get; set; }
    public bool ShowProgressInTray { get; set; }

    public void DeleteTimeSpan(TimeSlot toDel)
    {
        if (toDel.IsSystemPreserved)
        {
            Console.WriteLine("timeslot is protected, ignored");
            return;
        }

        if (Global.MyTimer.IsStarted() && Global.MyTimer.Duration == toDel.ToTimeSpan())
        {
            var msg = new Notification("operation not permitted", "the time slot is being used", NotificationType.Warning,
                TimeSpan.FromSeconds(3));
            this.SettingTabNotificationManager?.Show(msg);
            return;
        }

        for (int i = 0; i < AllTimeSlots.Count; i++)
        {
            if (AllTimeSlots[i].TotalSeconds() == toDel.TotalSeconds())
            {
                if (AllTimeSlots[i].IsSystemPreserved) //double check to prevent built-in item not being removed
                {
                    continue;
                }
                AllTimeSlots.RemoveAt(i);
                this.IndexOfSelectedTimeInSetting = i - 1; //select on previous item

                //if working space combo box has nothing selected
                if (IndexOfSelectedTimeInWorkspace > AllTimeSlots.Count - 1 || IndexOfSelectedTimeInWorkspace < 0)
                {
                    //workspace selected item is ok to update because a working item will not be deleted 
                    IndexOfSelectedTimeInWorkspace = AllTimeSlots.Count - 1;
                }

                SyncSettings();
                return;
            }
        }
    }

    public void AddTimeWindow()
    {
       
        //AddTimeDialog use the same context
        //We set close action here so that other method that bind to the View can call it directly. For instance, AddTimeSpan()
        AddTimeDialog addTimeDiag = new AddTimeDialog(this);
        this.addTimeSpanOkAction = addTimeDiag.Close;

        var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        addTimeDiag.Focus();
        addTimeDiag.BringIntoView();
        addTimeDiag.ShowDialog(mainWindow);
    }
    
    private event Action addTimeSpanOkAction;

    public void AddTimeSpan(decimal? toAdd)
    {

        if (toAdd == null || toAdd < 1 || toAdd > 60)
        {
            Console.WriteLine("illegal toAdd value ignored");
            return;
        }
        var toAddInt = (int)toAdd;


        foreach (var ts in AllTimeSlots)
        {
            if (ts.TotalSeconds() == toAddInt)
            {
                Console.WriteLine("repeated item will be ignored");
                return;
            }
        }
        AllTimeSlots.Add(new TimeSlot(toAddInt, false));
        TimeToAdd = null;//reset the view
        SyncSettings();

        int currIdx = AllTimeSlots.Count - 1;
        for (int i = 0; i < AllTimeSlots.Count; i++)
        {
            if ((int)AllTimeSlots[i].ToTimeSpan().TotalMinutes == toAddInt)
            {
                currIdx = i;
                break;
            }

        }

        IndexOfSelectedTimeInSetting = currIdx;
        if (!Global.MyTimer.IsStarted())
        {
            IndexOfSelectedTimeInWorkspace = currIdx;
        }
        Console.WriteLine($"{toAdd} added..");

        addTimeSpanOkAction?.Invoke();
    }

    private Settings ToSettingsModel()
    {
        return new Settings()
        {
            CloseWithoutExit = CloseWithoutExit,
            PlaySound = PlaySound,

            LastUsedIndex = IndexOfSelectedTimeInWorkspace,
            TimeSlots = AllTimeSlots.ToList(),
            ShowProgressInTry = ShowProgressInTray

        };

    }

    public void Fill(Settings settings)
    {
        this.CloseWithoutExit = settings.CloseWithoutExit;
        this.PlaySound = settings.PlaySound;
        this.AllTimeSlots = new ObservableCollection<TimeSlot>(settings.TimeSlots);
        this.IndexOfSelectedTimeInWorkspace = settings.LastUsedIndex;
        this.ShowProgressInTray = settings.ShowProgressInTry;
    }

    public void SyncSettings()
    {
        lock (_mainLock)
        {
            var current = ToSettingsModel();
            if (Global.LoaddedSetting.HasChanged(current))
            {
                Console.WriteLine("setting has changed");
                current.SaveToDisk(Global.ConfigPath);
                Global.LoaddedSetting = current;
            }
        }
    }

    public void StartCmd()
    {
        var tm = Global.MyTimer;
        var selected = AllTimeSlots[IndexOfSelectedTimeInWorkspace];

        tm.Init(DateTime.Now, selected.ToTimeSpan(), ProgressUpdateAction, TimeupAction);

        //update to full before start which will be reset to 0
        ProgressValue = 100;
        ProgressVisible = true;


        tm.Start();

        IsTimeSlotComboBoxEnabled = false;
        IsStartButtonEnabled = false;

        SyncSettings();
    }

    public void CancelCmd()
    {
        Global.MyTimer.Stop();
        ProgressVisible = false;
        IsStartButtonEnabled = true;
        IsTimeSlotComboBoxEnabled = true;

        lock (_mainLock)
        {
            //potential memory leak here
            _trayHelper.ResetTrayIcon();
        }
    }

    private void ProgressUpdateAction(double leftPercent100)
    {
        lock (_mainLock)
        {
            var tm = Global.MyTimer;
            if (!tm.IsStarted())
            {
                return;
            }
            var used = DateTime.Now.Subtract(tm.StartTime).Duration();
            var left = tm.Duration.Subtract(used).Duration();
            ShowProgressText = left.ToString(@"mm\:ss");
            ProgressValue = leftPercent100;
            if (ShowProgressInTray)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    _trayHelper.SetPercentageTrayIcon(leftPercent100);
                });
            }

        }
    }
    private async void TimeupAction()
    {
        await Dispatcher.UIThread.Invoke(async () =>
         {

             if (PlaySound)
             {
                 var player = new Player();
                 var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "notification.mp3");
                 await player.SetVolume(5);
                 await player.Play(path);
             }

             var box = new AlertBox();
             var result = await box.ShowAlertAsync();
             var args = new RoutedEventArgs();
             if (result == "Restart")
             {
                 if (Global.MyTimer.IsStarted())
                 {
                     return;
                 }
                 CancelCmd();
                 StartCmd();
             }
             else if (result == "Ok")
             {
                 if (Global.MyTimer.IsStarted())
                 {
                     return;
                 }
                 CancelCmd();
             }
             //potential memory leak here unless the box is closed rather than ok or restart button clicked
             box.Close();
         });

    }


}

public class ObservableViewModelBase : ObservableObject;