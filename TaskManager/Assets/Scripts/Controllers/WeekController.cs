using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeekController
{
    public int weekNumber { get; set; }

    public bool isWeekDone = false;

    public List<DayController> days = new List<DayController>();

    public float DonePercent()
    {
        int doneTaskCount = days.Where(t => t.isDayDone).Count();

        int result = doneTaskCount / days.Count();

        return result;
    }
}
