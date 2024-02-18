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


        public static List<TimeSpan> SysTimeSlots = new List<TimeSpan>{
            TimeSpan.FromSeconds(10),
            TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(15),
            TimeSpan.FromMinutes(20),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(60),
            TimeSpan.FromMinutes(120),
            TimeSpan.FromMinutes(180)
        };
    }

}