using System;
using System.Threading;
using System.Threading.Tasks;


namespace iTimeSlot.Shared
{
    //ProgressBarUpdateDelegate is the delegate func of ProgressBar.ProgressTo as they have identical signature
    public delegate void ProgressBarUpdateDelegate(double value);

    public delegate void OnTimeUpDelegate();

    internal static class Global
    {
        public static Timer MyTimer = new Timer();
    }

    public class Timer
    {
        public DateTime StartTime { get; set; }
        private TimeSpan Duration { get; set; }
        private DateTime EndTime { get; set; }

        // public Func<double, uint, Easing, Task<bool>>? ProgessUpdater;
        private ProgressBarUpdateDelegate? ProgessUpdater;
        private OnTimeUpDelegate? onTimeupCallback;


        private bool _isStarted = false;

        public void Init(DateTime startTime, TimeSpan duration, ProgressBarUpdateDelegate updateFunc, OnTimeUpDelegate onTimeupFunc)
        {
            this.Stop();
            StartTime = startTime;
            Duration = duration;
            EndTime = startTime.Add(duration);
            //Console.WriteLine("start time: " + StartTime, "Duration: " + Duration, "End time: " + EndTime);
            ProgessUpdater = updateFunc;
            onTimeupCallback = onTimeupFunc;
        }

        public bool IsTimeUp()
        {
            return DateTime.Now > EndTime;
        }
        public bool IsStarted()
        {
            return _isStarted;
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
                        //Console.WriteLine("Time up callback");
                        onTimeupCallback?.Invoke();
                        //Console.WriteLine("Time up callback done");
                        Stop();
                        return;
                    }
                    Thread.Sleep(1000);
                    var remainProgress = 100*(EndTime - DateTime.Now).TotalSeconds / Duration.TotalSeconds;
                    ProgessUpdater?.Invoke(remainProgress);
      
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
            ProgessUpdater = null;
            onTimeupCallback = null;
            return _isStarted;
        }
    }
}