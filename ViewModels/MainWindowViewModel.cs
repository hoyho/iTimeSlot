

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace iTimeSlot.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

    public List<TimeSpan> ExistingTimeSpans => Shared.Global.ExistTimeSpans;

}
