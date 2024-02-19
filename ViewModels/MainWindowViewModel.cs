using System;
using System.Collections.Generic;

namespace iTimeSlot_avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static
    
    //public List<TimeSpan> AllTimeSpans;
}
