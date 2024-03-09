using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using iTimeSlot.Models;

namespace iTimeSlot.ViewModels;

public partial class MainWindowViewModel : ObservableViewModelBase
{

    private ObservableCollection<TimeSlot> _slots;
    public ObservableCollection<TimeSlot> AllTimeSlots
    {
        get { return _slots; }
        set { SetProperty(ref _slots, value); }
    }
    
    
    private decimal? _time2Add ;

    public decimal? TimeToAdd
    {
        get { return _time2Add; }
        set { this.SetProperty(ref _time2Add, value); }
    }
    
    
    private int _indexOfSelectedTime ;
    public int IndexOfSelectedTime
    {
        get { return _indexOfSelectedTime; }
        set { this.SetProperty(ref _indexOfSelectedTime, value); }
    }

    // public ICommand DeleteCommand { get; }
    
    public void DeleteTimeSpan(TimeSlot toDel)
    {
        if (toDel.IsSystemPreserved)
        {
            Console.WriteLine("timeslot is protected, ignored");
        }
        
        for (int i = 0; i < AllTimeSlots.Count; i++)
        {
            if (AllTimeSlots[i].TotalSeconds() == toDel.TotalSeconds())
            {
                AllTimeSlots.RemoveAt(i);
                this.IndexOfSelectedTime = i - 1; //select on previous item
                return;
            }
        }
    }
    
    public void AddTimeSpan(decimal? toAdd)
    {
        
        if (toAdd ==null || toAdd <= 1)
        {
            Console.WriteLine("illegal toAdd value ignored");
            return;
        }
        var toAddInt = (int)toAdd;
        
        AllTimeSlots.Add(new TimeSlot(toAddInt,false));
        TimeToAdd = null;//reset the view
        
        int currIdx = AllTimeSlots.Count -1;
        for (int i = 0; i < AllTimeSlots.Count; i++)
        {
            if ((int)AllTimeSlots[i].ToTimeSpan().TotalMinutes == toAddInt)
            {
                currIdx = i;
                break;
            }
            
        }
        
        IndexOfSelectedTime = currIdx;
        Console.WriteLine($"{toAdd} added..");
    }
    
}

public class ObservableViewModelBase : ObservableObject;