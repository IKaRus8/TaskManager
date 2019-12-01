using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeekController
{
    public string WeekName { get; set; }
    public int weekNumber { get; set; }

    public bool isWeekDone = false;

    public List<DayController> days = new List<DayController>();

    public WeekController(int number, string weekName)
    {
        weekNumber = number;
        WeekName = weekName + $"({weekNumber})";

        foreach(DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            DayController newDay = new DayController(day);

            days.Add(newDay);
        }
    }

    public void AddTask(DayController day, BaseTask task)
    {
        day.tasks.Add(task);

        DonePercent();
    }

    public float DonePercent()
    {
        int doneTaskCount = days.Where(t => t.isDayDone).Count();

        int result = doneTaskCount / days.Count();

        return result;
    }
}
