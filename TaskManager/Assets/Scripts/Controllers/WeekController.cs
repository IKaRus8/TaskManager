using System;
using System.Collections.Generic;
using System.Linq;

public class WeekController
{
    public string WeekName { get; set; }
    public int weekNumber { get; set; }

    public DateTime startWeek;

    public bool isWeekDone;

    private List<DayController> days = new List<DayController>();

    public WeekController(int number, string weekName)
    {
        weekNumber = number;
        WeekName = weekName + $"({weekNumber})";

        FillDays();
    }

    public void FillDays()
    {
        days = new List<DayController>();

        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            DayController newDay = new DayController(day);

            days.Add(newDay);
        }
    }

    public void AddTask(TaskInfo task)
    {
        var day = days.FirstOrDefault(d => d.DayOfWeek == task._dayOfWeek);

        day.AddTask(task);

        DonePercent();
    }

    public float DonePercent()
    {
        int doneTaskCount = days.Where(t => t.isDayDone).Count();

        int result = doneTaskCount / days.Count();

        return result;
    }

    public DayController GetDay(DayOfWeek dayOfWeek)
    {
        var day = days.FirstOrDefault(d => d.DayOfWeek == dayOfWeek);

        return day;
    }

    public void SetDayDate()
    {

    }

    public void UpdateWeek()
    {
        days.ForEach(d => d.UpdateTasks());

        isWeekDone = false;
    }
}
