using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using iTimeSlot.Models;

namespace iTimeSlot.Foundation;

public interface IStatistics
{
    DailyStat ReadTodayData();

    //ReadWeekData return most recent statistics data
    List<DailyStat> ReadWeekData(int max);

    //CompleteTask marks a new task completed and record it for later use.
    void CompleteTask(int minute);

    //CompleteTask marks a new break interval completed and record it for later use.
    void CompleteBreak(int minute);
}


//DiskStatistics is a class that implements IStatistics interface using disk as storage
public class DiskStatistics : IStatistics
{
    private const string TimeslotDateFormat = "yyyy-MM-dd"; //date format: 2024-06-01
    private readonly string _dataPath;

    public DiskStatistics(string dataPath)
    {
        _dataPath = dataPath;
    }

    private void EnsureExist()
    {
        if (string.IsNullOrEmpty(_dataPath))
        {
            throw new Exception("Data path is not set");
        }
        var parentDir = Path.GetDirectoryName(_dataPath);
        if (string.IsNullOrEmpty(parentDir))
            throw new Exception("Invalid Parent directory");

        if (!Directory.Exists(parentDir))
        {
            Directory.CreateDirectory(parentDir);
        }

        if (!File.Exists(_dataPath))
        {
            File.WriteAllText(_dataPath, "{}");
        }
    }

    public void CompleteBreak(int minute)
    {
        EnsureExist();
        LogFinishedInterval(IntervalType.Break, minute);
    }

    public void CompleteTask(int minute)
    {
        EnsureExist();
        LogFinishedInterval(IntervalType.Work, minute);
    }



    private void LogFinishedInterval(IntervalType type, int minute)
    {
        //read from disk and update
        try
        {
            string jsonString = File.ReadAllText(_dataPath);
            var stats = JsonSerializer.Deserialize(jsonString, new JsonContext().Stats);
            if (stats == null)
            {
                stats = new Stats()
                {
                    DailyStats = new List<DailyStat>() { }
                };
            }

            var record = stats.DailyStats.FirstOrDefault(s => s.Date == DateTime.Today.ToString(TimeslotDateFormat));
            bool notExist = record == null;
            if (record == null)
            {
                record = new DailyStat()
                {
                    Date = DateTime.Today.ToString(TimeslotDateFormat),
                };
            }

            //update entry
            if (type == IntervalType.Work)
            {
                record.WorkCount += 1;
                record.TotalWorkMinutes += minute;
            }
            else
            {
                record.BreakCount += 1;
                record.TotalBreakMinutes += minute;
            }

            //add to list if not existed 
            if (notExist)
            {
                stats.DailyStats.Add(record);
            }

            var json = JsonSerializer.Serialize(stats, new JsonContext().Stats);
            File.WriteAllText(_dataPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to update db:" + ex);
        }

    }

    public DailyStat ReadTodayData()
    {
        EnsureExist();
        string jsonString = File.ReadAllText(_dataPath);
        var stats = JsonSerializer.Deserialize(jsonString, new JsonContext().Stats);

        var ds = new DailyStat()
        {
            Date = DateTime.Today.ToString(TimeslotDateFormat),
        };

        if (stats == null || stats.DailyStats == null)
        {
            return ds;
        }

        var existed = stats.DailyStats.FirstOrDefault(s => s.Date == DateTime.Today.ToString(TimeslotDateFormat));
        if (existed != null)
        {
            ds = existed;
        }

        return ds;
    }

    public List<DailyStat> ReadWeekData(int max = 7)
    {
        EnsureExist();

        string jsonString = File.ReadAllText(_dataPath);
        var stats = JsonSerializer.Deserialize(jsonString, new JsonContext().Stats);

        var rs = new List<DailyStat>();

        if (stats == null || stats.DailyStats == null)
        {
            return rs;
        }

        var existed = stats.DailyStats.OrderByDescending(s => s.Date).Take(max);

        //ensure return in dated order
        return existed.OrderBy(d => d.Date).ToList();
    }
}