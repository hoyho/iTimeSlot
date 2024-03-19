using System;
using System.Threading;
using System.Threading.Tasks;

namespace iTimeSlot.Shared
{
    //OnProgressUpdateDelegate is the delegate func of ProgressBar.ProgressTo as they have identical signature
    public delegate void OnProgressUpdateDelegate(double value);
    public delegate void OnTimeUpDelegate();
    
    public class Timer
    {
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        private DateTime EndTime { get; set; }


        private OnProgressUpdateDelegate? _progressCallback;
        private OnTimeUpDelegate? _timeupCallback;


        private bool _isStarted = false;

        public void Init(DateTime startTime, TimeSpan duration,
        OnProgressUpdateDelegate onProgressUpdateFunc, OnTimeUpDelegate onTimeupFunc)
        {
            this.Stop();
            StartTime = startTime;
            Duration = duration;
            EndTime = startTime.Add(duration);
            //Console.WriteLine("start time: " + StartTime, "Duration: " + Duration, "End time: " + EndTime);
            _progressCallback = onProgressUpdateFunc;
            _timeupCallback = onTimeupFunc;
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
                        _timeupCallback?.Invoke();
                        //Console.WriteLine("Time up callback done");
                        Stop();
                        return;
                    }
                    Thread.Sleep(1000);
                    var remainProgress = 100*(EndTime - DateTime.Now).TotalSeconds / Duration.TotalSeconds;
                    _progressCallback?.Invoke(remainProgress);
      
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