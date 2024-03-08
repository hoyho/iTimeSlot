

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace iTimeSlot.ViewModels;

public class WorkspaceTabViewModel : ViewModelBase
{

    public List<TimeSpan> ExistingTimeSpans => Shared.Global.ExistTimeSpans;

}
