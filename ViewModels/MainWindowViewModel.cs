using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    
    
}

public class ObservableViewModelBase : ObservableObject;