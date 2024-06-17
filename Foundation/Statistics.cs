using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DynamicData;
using DynamicData.Kernel;
using iTimeSlot.Models;

public interface IStatistics
{
    DailyStat ReadTodayData();
    void ReadWeekData();

    void CompleteTask(int minute);
    void CompleteBreak(int minute);
}


//DiskStatistics is a class that implements IStatistics interface using disk as storage
public class DiskStatistics : IStatistics
{
    private string iTimeslotDateFormat ="yyyy-MM-dd"; //date format: 2024-06-01
    private string _dataPath = "";

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
            else if (stats.DailyStats == null)
            {
                stats.DailyStats = new List<DailyStat>() { };
            }

            var entry = new DailyStat()
            {
                Date = DateTime.Today.ToString(iTimeslotDateFormat),
            };

            var existed = stats.DailyStats.FirstOrDefault(s => s.Date == DateTime.Today.ToString(iTimeslotDateFormat));
            if (existed != null)
            {
                entry = existed;
            }

            //update entry
            if (type == IntervalType.Work)
            {
                entry.WorkCount += 1;
                entry.TotalWorkMinutes += minute;
            }
            else
            {
                entry.BreakCount += 1;
                entry.TotalBreakMinutes += minute;
            }

            //add to list if not existed 
            if (existed == null)
            {
                stats.DailyStats.Add(entry);
            }

            var json = JsonSerializer.Serialize(stats, new JsonContext().Stats);
            File.WriteAllText(_dataPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to update db:" + ex.Message);
        }

    }

    public DailyStat ReadTodayData()
    {
        EnsureExist();
        string jsonString = File.ReadAllText(_dataPath);
        var stats = JsonSerializer.Deserialize(jsonString, new JsonContext().Stats);

        var ds = new DailyStat()
        {
            Date = DateTime.Today.ToString(iTimeslotDateFormat),
        };

        if (stats == null || stats.DailyStats == null)
        {
            return ds;
        }

        var existed = stats.DailyStats.FirstOrDefault(s => s.Date == DateTime.Today.ToString(iTimeslotDateFormat));
        if (existed != null)
        {
            ds = existed;
        }

        return ds;
    }

    public void ReadWeekData()
    {
        EnsureExist();
        throw new NotImplementedException();
    }
}
