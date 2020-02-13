using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DayController
{
    public DateTime Date { get; set; }
    public DayOfWeek DayOfWeek { get; set; }

    public bool isDayDone = false;

    public List<BaseTask> tasks = new List<BaseTask>();

    public DayController(DayOfWeek day)
    {
        DayOfWeek = day;
    }

    public void AddTask(TaskItem task, string weekNane)
    {
        task._weekname = weekNane;
        task._dayOfWeek = DayOfWeek;

        tasks.Add(task);

        DonePercent();
    }

    public void AddDisposableTask(BaseTask task)
    {
        tasks.Add(task);

        DonePercent();
    }

    public float DonePercent()
    {
        int doneTaskCount = tasks.Where(t => t.IsDone).Count();

        int result = doneTaskCount / tasks.Count();

        return result;
    }
}
