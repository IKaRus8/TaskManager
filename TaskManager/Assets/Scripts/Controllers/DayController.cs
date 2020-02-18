using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DayController
{
    public DateTime Date { get; set; }
    public DayOfWeek DayOfWeek { get; set; }

    public bool isDayDone;

    private List<TaskInfo> tasks = new List<TaskInfo>();

    public DayController(DayOfWeek day)
    {
        DayOfWeek = day;
    }

    public void AddTask(TaskInfo task)
    {
        tasks.Add(task);

        DonePercent();
    }

    public float DonePercent()
    {
        int doneTaskCount = tasks.Where(t => t._isDone).Count();

        int result = doneTaskCount / tasks.Count();

        return result;
    }
}
