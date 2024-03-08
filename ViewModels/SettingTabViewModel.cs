

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace iTimeSlot.ViewModels;

public class SettingTabViewModel : ViewModelBase
{
    
    
    // private ObservableCollection<TimeSpan> _myTimespans { get; set; }
    //
    // public ObservableCollection<TimeSpan> ExistingTimeSpans
    // {
    //     get { return _myTimespans; }
    //     set { _myTimespans = value; }
    // }
    public List<TimeSpan> ExistingTimeSpans => Shared.Global.ExistTimeSpans;
}
