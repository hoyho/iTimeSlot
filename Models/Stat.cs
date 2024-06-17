

using System;
using System.Collections.Generic;

public class DailyStat
{
    public string Date { get; set; }

    //WorkCount is the number of work intervals completed
    public int WorkCount { get; set; }

    //TotalWorkMinutes is the total minutes spent on work
    public int TotalWorkMinutes { get; set; }

    //BreakCount is the number of break intervals completed
    public int BreakCount { get; set; }

    //TotalBreakMinutes is the total minutes spent on break
    public int TotalBreakMinutes { get; set; }

}

public class Stats
{
    public List<DailyStat> DailyStats { get; set; }
}