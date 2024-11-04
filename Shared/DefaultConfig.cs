using System;
using System.Collections.Generic;

namespace iTimeSlot.Shared
{

    public static class DefaultConfig
    {
        public static readonly List<TimeSpan> SysWorkTimeSlots = new List<TimeSpan>{
            TimeSpan.FromMinutes(25),
            TimeSpan.FromMinutes(15),
            TimeSpan.FromMinutes(30)
        };
        public static readonly List<TimeSpan> SysBreakTimeSlots = new List<TimeSpan>{
            TimeSpan.FromMinutes(5),
        };

        public static readonly bool SysCloseWithoutExit = true;
        public static readonly bool SysPlaySound = false;
        public static readonly bool SysShowProgInTray = true;
    }

}