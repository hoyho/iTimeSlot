using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace iTimeSlot.Models
{
    public class TimeSlot: ObservableObject
    {
        private TimeSpan _ts;
        
        public TimeSlot(TimeSpan srcTs)
        {
            this._ts = srcTs;
        }

        public TimeSlot(int minute) : this(TimeSpan.FromMinutes(minute))
        {
            
        }

        public override string ToString()
        {
            return  $"{(int) _ts.TotalMinutes} min";
        }

        public TimeSpan ToTimeSpan()
        {
            return _ts;
        }
        
    }
}
