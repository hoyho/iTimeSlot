using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using iTimeSlot.Models;
using iTimeSlot.Shared;

namespace iTimeSlot.ViewModels;

public partial class MainWindowViewModel : ObservableViewModelBase
{

    private ObservableCollection<TimeSlot> _slots;
    public ObservableCollection<TimeSlot> AllTimeSlots
    {
        get { return _slots; }
        set { SetProperty(ref _slots, value); }
    }
    
    private readonly object _mainLock = new object();
    
    
    private decimal? _time2Add ;

    public decimal? TimeToAdd
    {
        get { return _time2Add; }
        set { this.SetProperty(ref _time2Add, value); }
    }
    
    private int _indexOfTimeInWorkspace ;
    
    public int IndexOfSelectedTimeInWorkspace
    {
        get { return _indexOfTimeInWorkspace; }
        set { this.SetProperty(ref _indexOfTimeInWorkspace, value); }
    }
    
    private int _indexOfTimeInSetting ;
    public int IndexOfSelectedTimeInSetting
    {
        get { return _indexOfTimeInSetting; }
        set { this.SetProperty(ref _indexOfTimeInSetting, value); }
    }
    
    public WindowNotificationManager? SettingTabNotificationManager { get; set; }

    public bool CloseWithoutExit { get; set; }
    public bool PlaySound { get; set; }
    
    public void DeleteTimeSpan(TimeSlot toDel)
    {
        if (toDel.IsSystemPreserved)
        {
            Console.WriteLine("timeslot is protected, ignored");
            return;
        }
        //todo make it more reliable
        //    if (AllTimeSlots[_indexOfTimeInWorkspace].ToTimeSpan() == toDel.ToTimeSpan())
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
    
    public void AddTimeSpan(decimal? toAdd)
    {
        
        if (toAdd ==null || toAdd < 1)
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
        AllTimeSlots.Add(new TimeSlot(toAddInt,false));
        TimeToAdd = null;//reset the view
        SyncSettings();
        
        int currIdx = AllTimeSlots.Count -1;
        for (int i = 0; i < AllTimeSlots.Count; i++)
        {
            if ((int)AllTimeSlots[i].ToTimeSpan().TotalMinutes == toAddInt)
            {
                currIdx = i;
                break;
            }
            
        }
        
        IndexOfSelectedTimeInSetting = currIdx;
        if(!Shared.Global.MyTimer.IsStarted()){
            IndexOfSelectedTimeInWorkspace = currIdx;
        }
        Console.WriteLine($"{toAdd} added..");
    }

    private Settings ToSettingsModel()
    {
        return new Settings()
        {
            CloseWithoutExit = CloseWithoutExit,
            PlaySound = PlaySound,
            
            LastUsedIndex = IndexOfSelectedTimeInWorkspace,
            TimeSlots = AllTimeSlots.ToList(),
        };
        
    }

    public void Fill(Settings settings)
    {
        this.CloseWithoutExit = settings.CloseWithoutExit;
        this.PlaySound = settings.PlaySound;
        this.AllTimeSlots = new ObservableCollection<TimeSlot>(settings.TimeSlots);
        this.IndexOfSelectedTimeInWorkspace = settings.LastUsedIndex;
        Console.WriteLine(this.IndexOfSelectedTimeInWorkspace);
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
    
}

public class ObservableViewModelBase : ObservableObject;