using System;
using System.Text.Json.Serialization;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace iTimeSlot.Models
{
    public class TimeSlot : ObservableObject
    {
        public bool IsSystemPreserved { get; set; }
        public IntervalType IntervalType { get; set; }


        public IBrush DisplayColor
        {
            get
            {
                if (this.IntervalType == IntervalType.Break)
                {
                    return new SolidColorBrush(Colors.LightGreen);
                }
                else
                {
                    return new SolidColorBrush(Colors.LightCoral);
                }
            }
        }

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

        public TimeSlot(TimeSpan srcTs, IntervalType iType, bool isSystemPreserved = false)
        {
            this._ts = srcTs;
            IsSystemPreserved = isSystemPreserved;
            IntervalType = iType;
        }

        public TimeSlot(int minute, IntervalType iType, bool isSystemPreserved = false) :
        this(TimeSpan.FromMinutes(minute),iType, isSystemPreserved)
        {

        }

        public override string ToString()
        {
            int m = (int)_ts.TotalMinutes;
            string unit = m > 1 ? "mins" : "min";
            return $"{(int)_ts.TotalMinutes} {unit}".TrimEnd();
        }

        public TimeSpan ToTimeSpan()
        {
            return _ts;
        }

        public int TotalSeconds()
        {
            return (int)_ts.TotalSeconds;
        }

    }
    public enum IntervalType
    {
        Work,
        Break
    }
}
