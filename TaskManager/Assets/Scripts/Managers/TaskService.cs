using Assets.Scripts.DI.Signals;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

/// <summary>
/// Сервис работы с задачами
/// </summary>
public class TaskService
{
    private UIManager _panelManager;
    private WeekManager _weekManager;
    private SignalBus _signalBus;

    private List<TaskInfo> ActiveTasks { get; set; }

    public TaskService(UIManager panelManager, WeekManager weekManager, SignalBus signalBus)
    {
        _panelManager = panelManager;
        _weekManager = weekManager;
        _signalBus = signalBus;

        _signalBus.Subscribe<TaskCreateSignal>(Create);
        _signalBus.Subscribe<TaskRemovedSignal>(OnTaskRemoved);

        ActiveTasks = new List<TaskInfo>();
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

    public void Save(TaskInfo newTask)
    {
        StorageManager.Save(newTask);
    }

    public void ShowTodayTasks()
    {
        DayController day = _weekManager.CurrentWeek.GetDay(DateTime.Now.DayOfWeek);

        string message = $"{TextStorage.Today} {_weekManager.CurrentWeek.WeekName}, {day.DayOfWeek}";

        _signalBus.Fire(new SendMessageSignal(MessageTarget.Header, message));

        day.tasks.ForEach(t => 
        {
            CreateTaskItem(t);
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
        if (!ActiveTasks.Any())
        {
            _signalBus.Fire(new SetActiveTutorialSignal(false)); 
        }

        var taskItem = _panelManager.CreatePanel<BaseTask>(_panelManager.taskConatainer);

        taskItem.Init(task);

        ActiveTasks.Add(task);
    }

    /// <summary>
    /// Создать новую задачу
    /// </summary>
    /// <param name="signal">Информация о задаче</param>
    private void Create(TaskCreateSignal signal)
    {
        if (signal.Info.isRecurring)
        {
            signal.Week.AddTask(signal.Info);
        }

        Save(signal.Info);
        CheckTaskToShow(signal.Info);
    }

    private void OnTaskRemoved(TaskRemovedSignal signal)
    {
        ActiveTasks.Remove(signal.Task);

        if (!ActiveTasks.Any())
        {
            _signalBus.Fire(new SetActiveTutorialSignal(true));
        }
    }
}