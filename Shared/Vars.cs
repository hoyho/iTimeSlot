using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Maui.Converters;
using CoreBluetooth;

namespace iTimeSlot.Shared
{
    //ProgressBarUpdateDelegate is the delagate func of ProgressBar.ProgressTo as they have identical signature
    public delegate Task<bool> ProgressBarUpdateDelegate(double value, uint length, Easing easing);

    public delegate void OnTimeUpDelegate();

    internal static class Global
    {
        public static Timer MyTimer = new Timer();
    }

    public class Timer
    {
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get; set; }

        // public Func<double, uint, Easing, Task<bool>>? ProgessUpdater;
        public ProgressBarUpdateDelegate? ProgessUpdater;
        public OnTimeUpDelegate? onTimeupCallback;


        private bool _isStarted = false;

        public void Init(DateTime startTime, TimeSpan duration, ProgressBarUpdateDelegate updateFunc, OnTimeUpDelegate onTimeupFunc)
        {
            this.Stop();
            StartTime = startTime;
            Duration = duration;
            EndTime = startTime.Add(duration);
            ProgessUpdater = updateFunc;
            onTimeupCallback = onTimeupFunc;
        }

        public bool IsTimeUp()
        {
            return DateTime.Now > EndTime;
        }

        public bool Start()
        {
            if (_isStarted)
            {
                return true;
            }

            _isStarted = true;


            Task.Factory.StartNew(() =>
            {
                while (_isStarted)
                {
                    if (IsTimeUp())
                    {
                        Console.WriteLine("Time up callback");
                        onTimeupCallback?.Invoke();
                        Console.WriteLine("Time up callback done");
                        break;
                    }
                    Thread.Sleep(1000);
                    var remainProgess = (EndTime - DateTime.Now).TotalSeconds / Duration.TotalSeconds;
                    ProgessUpdater?.Invoke(remainProgess, 500, Easing.Linear);
                }
            });
            return _isStarted;
        }

        public bool Stop()
        {
            if (_isStarted)
            {
                _isStarted = false;
            }
            return _isStarted;
        }
    }
}