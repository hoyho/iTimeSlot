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
        
        public static Settings LoaddedSetting  = new ();
        
        public static string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".iTimeSlot", "config.json");
        
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
            foreach (var st in DefaultConfig.SysTimeSlots)
            {
                defSetting.TimeSlots.Add(new TimeSlot(st,true));
            }
            
            defSetting.SaveToDisk(ConfigPath);
            return defSetting;
        }
    }

    }