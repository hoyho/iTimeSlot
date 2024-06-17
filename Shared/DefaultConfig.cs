using System;
using System.Collections.Generic;

namespace iTimeSlot.Shared
{

    public static class DefaultConfig
    {
        public static string[] RemindBefores = new string[]
        {
            "1 minute before",
            "2 minute before",
            "5 minute before",
            "10 minute before"
        };


        public static readonly List<TimeSpan> SysWorkTimeSlots = new List<TimeSpan>{
            TimeSpan.FromMinutes(25),
            TimeSpan.FromMinutes(60)
        };
        public static readonly List<TimeSpan> SysBreakTimeSlots = new List<TimeSpan>{
            TimeSpan.FromMinutes(5),
        };

        public static readonly bool SysCloseWithoutExit = true;
        public static readonly bool SysPlaySound = false;
        public static readonly bool SysShowProgInTray = true;
    }

}