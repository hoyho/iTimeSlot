using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using iTimeSlot.Models;


namespace iTimeSlot.Shared
{
    internal static class Global
    {
        public static Timer MyTimer = new Timer();

        public static Settings LoaddedSetting = new();

        public static string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".iTimeSlot", "config.json");
        private static string statPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".iTimeSlot", "stats.json");
        public static IStatistics StatReporter = new DiskStatistics(statPath);

        public static Settings EnsureDefaultConfigFile()
        {
            var defSetting = new Settings()
            {
                CloseWithoutExit = DefaultConfig.SysCloseWithoutExit,
                LastUsedIndex = 0,
                PlaySound = DefaultConfig.SysPlaySound,
                ShowProgressInTry = DefaultConfig.SysShowProgInTray
            };

            defSetting.TimeSlots = new List<TimeSlot>();
            foreach (var st in DefaultConfig.SysWorkTimeSlots)
            {
                defSetting.TimeSlots.Add(new TimeSlot(st, IntervalType.Work, true));
            }
            foreach (var st in DefaultConfig.SysBreakTimeSlots)
            {
                defSetting.TimeSlots.Add(new TimeSlot(st, IntervalType.Break, true));
            }

            defSetting.SaveToDisk(ConfigPath);
            return defSetting;
        }
    }

}