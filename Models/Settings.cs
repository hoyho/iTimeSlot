using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace iTimeSlot.Models
{
    public class Settings: ObservableObject
    {
        //keep this public parameterless constructor for json serialization and deserialization
        [JsonConstructor]
        public Settings() {}
        
        public List<TimeSlot> TimeSlots { get; set; }
        
        public int LastUsedIndex { get; set; }

        public bool CloseWithoutExit { get; set; }

        public bool PlaySound { get; set; }
        
        public bool ShowProgressInTry { get; set; }


       public void SaveToDisk(string path)
       {
           try
           {
               var parentPath = Path.GetDirectoryName(path);
               if (parentPath != null && !File.Exists(parentPath))
               {
                   Directory.CreateDirectory(parentPath);
               }
               var json = JsonSerializer.Serialize(this, new JsonContext().Settings);
               Console.WriteLine("save " +json + " to " + path);
               File.WriteAllText(path,json);
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
       }
       
       public bool HasChanged(Settings target)
       {
           //todo refactor with reflection( it seems to buggy in macOS app when using reflection
           if (target.PlaySound != this.PlaySound)
           {
               return true;
           }

           if (target.CloseWithoutExit != CloseWithoutExit)
           {
               return true;
           }
           
           if (target.LastUsedIndex != this.LastUsedIndex)
           {
               return true;
           }
           
           if (target.ShowProgressInTry != this.ShowProgressInTry)
           {
               return true;
           }

           if (target.TimeSlots.ToList().Order() != this.TimeSlots.ToList().Order())
           {
               return true;
           }

  
           return false;
       }
    }
}
