﻿

using System;
using System.Collections.Generic;

namespace iTimeSlot.ViewModels;

public class SettingTabViewModel : ViewModelBase
{
    public List<TimeSpan> ExistingTimeSpans => Shared.Global.ExistTimeSpans;
}
