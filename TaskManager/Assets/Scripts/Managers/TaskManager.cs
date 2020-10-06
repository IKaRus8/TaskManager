using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public TaskCreatePanel _taskCreatePanel { get; set; }

    private PanelManager _panelManager => PanelManager.Instance;
    private WeekManager _weekManager => WeekManager.Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void LoadTasks()
    {
        List<TaskInfo> tasks = StorageManager.LoadTasksForUser();

        tasks.ForEach(t => 
        {
            if (t.isRecurring)
            {
                WeekController week = _weekManager.Weeks.FirstOrDefault(w => w.WeekName == t._weekName);

                if (week != null)
                {
                    week.AddTask(t);
                }
            }
            else
            {
                CheckTaskToShow(t);
            }
        });
    }

    public void Create(WeekController week, TaskInfo newTask)
    {
        if (newTask.isRecurring)
        {
            week.AddTask(newTask);
        }

        Save(newTask);
        CheckTaskToShow(newTask);
    }

    public void Save(TaskInfo newTask)
    {
        StorageManager.Save(newTask);
    }

    public void ShowTodayTasks()
    {
        DayController day = _weekManager.CurrentWeek.GetDay(DateTime.Now.DayOfWeek);

        MessageManager.SetHeaderCaption($"{TextStorage.Today} {_weekManager.CurrentWeek.WeekName}, {day.DayOfWeek.ToString()}");

        day.tasks.ForEach(t => 
        {
            BaseTask taskItem = _panelManager.CreatePanel<BaseTask>(_panelManager.taskConatainer.transform);

            taskItem.Construct(t);
        });
    }

    public void CheckTaskToShow(TaskInfo task)
    {
        if (task.isRecurring)
        {
            if (task._weekName == _weekManager.CurrentWeek.WeekName && task._dayOfWeek == DateTime.Now.DayOfWeek)
            {
                CreateTaskItem(task);
            } 
        }
        else
        {
            if(task._date.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                CreateTaskItem(task);
            }
            else if (task._date < DateTime.Now)
            {
                task.deleted = true;

                StorageManager.Update(task);
            }
        }
    }

    private void CreateTaskItem(TaskInfo task)
    {
        MessageManager.ShowHideTutorial(true);

        var taskItem = _panelManager.CreatePanel<BaseTask>(_panelManager.taskConatainer.transform);

        taskItem.Construct(task);
    }
}