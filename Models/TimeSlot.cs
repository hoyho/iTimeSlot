using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace iTimeSlot.Models
{
    public class TimeSlot: ObservableObject
    {
        public bool IsSystemPreserved {get; set; }
        
        private TimeSpan _ts;
        
        [JsonPropertyName("TimeSpan")]
        public TimeSpan Ts
        {
            get { return _ts; }
            set { _ts = value; }
        }

        
        //keep this public parameterless constructor for json serialization and deserialization
        [JsonConstructor]
        public TimeSlot()
        {
            
        }
        
        public TimeSlot(TimeSpan srcTs, bool isSystemPreserved=false)
        {
            this._ts = srcTs;
            IsSystemPreserved = isSystemPreserved;
        }

        public TimeSlot(int minute, bool isSystemPreserved=false) : this(TimeSpan.FromMinutes(minute),isSystemPreserved)
        {
            
        }

        public override string ToString()
        {
            return  $"{(int) _ts.TotalMinutes} min".TrimEnd();
        }

        public TimeSpan ToTimeSpan()
        {
            return _ts;
        }

        public int TotalSeconds()
        {
            return (int) _ts.TotalSeconds;
        }
        
    }
}
