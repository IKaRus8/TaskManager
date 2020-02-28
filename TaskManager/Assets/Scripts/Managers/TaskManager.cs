using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<WeekController> Weeks { get; set; }

    private TaskCreatePanel taskCreatePanel;

    private PanelManager _panelManager => PanelManager.Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Weeks = new List<WeekController>();
    }

    public void CreateTask()
    {

    }

    public void AddWeek(WeekController newWeek)
    {
        if (newWeek != null)
        {
            newWeek.FillDays();

            Weeks.Add(newWeek); 
        }
    }

    public void AddWeek(string weekName)
    {
        WeekController newWeek = new WeekController(Weeks.Count + 1, weekName);

        if (Weeks.Any())
        {
            newWeek.startWeek = GetNextWeekStartDay(Weeks.Last().startWeek);
        }
        else
        {
            DateTime input = new DateTime(2020, 1, 6, 10, 0, 0); //DateTime.Now;
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            newWeek.startWeek = input.AddDays(delta);
        }

        Weeks.Add(newWeek);

        StorageManager.Update(newWeek);
    }

    public void Save()
    {
        StorageManager.Save();
    }

    public void OnAuthorization(List<WeekController> weeks)
    {
        if (weeks != null && weeks.Any())
        {
            weeks.OrderBy(w => w?.startWeek)?.ToList().ForEach(w => AddWeek(w));

            LoadTasks();

            ShowTodayTasks(GetCurrentWeek()); 
        }
    }

    public void LoadTasks()
    {
        var tasks = StorageManager.Load();

        foreach(var task in tasks)
        {
            var week = Weeks.FirstOrDefault(w => w.WeekName == task._weekName);

            if (week != null)
            {
                week.AddTask(task);
            }
        }
    }

    public void WeekDialogConstruct()
    {
        var panel = _panelManager.CreatePanel<DialogPanel>(_panelManager.panelBack.transform);

        if (panel != default)
        {
            panel.input.text = $"Неделя {Weeks.Count + 1}";
            panel.Action = OnWeekCreate;

            _panelManager.SwitchOffPanels();
            _panelManager.EnableBackground(true);
        }
    }

    private void OnWeekCreate(string text)
    {
        AddWeek(text);

        //var calendar = _panelManager.GetPanel<CalendarController>();
        //_panelManager.EnableBackground(true);
        //calendar.Show();

        //get monday ---------------------------------------------------------------------------------------------
        //System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
        //DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
        //DayOfWeek today = DateTime.Now.DayOfWeek;
        //DateTime sow = DateTime.Now.AddDays(-(today - fdow)).Date;
    }

    public void ShowTaskCreatePanel()
    {
        _panelManager.SwitchOffPanels();
        _panelManager.EnableBackground(true);

        taskCreatePanel = _panelManager.CreatePanel<TaskCreatePanel>(_panelManager.panelBack.transform);
        taskCreatePanel.Show();
    }

    public static DateTime GetNextWeekStartDay(DateTime start, DayOfWeek day = DayOfWeek.Monday)
    {
        start = start.AddDays(1);

        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
        return start.AddDays(daysToAdd);
    }

    public WeekController GetCurrentWeek()
    {
        var week = Weeks.FirstOrDefault(w => w.weekNumber == Weeks.Where(w1 => w1.startWeek <= DateTime.Now).Max(w2 => w2.weekNumber));

        if(week != null && week.startWeek.AddDays(7) < DateTime.Now)
        {
            Weeks = Weeks.OrderBy(w => w.weekNumber).ToList();

            Weeks.FirstOrDefault().startWeek = GetNextWeekStartDay(week.startWeek);

            for (int i = 1; i < Weeks.Count; i++)
            {
                Weeks[i].startWeek = GetNextWeekStartDay(Weeks[i - 1].startWeek);
            }

            UpdateWeeks();

            GetCurrentWeek();
        }

        return week;
    }

    private void UpdateWeeks()
    {
        Weeks.ForEach(w => w.UpdateWeek());
    }

    public void ShowTodayTasks(WeekController week)
    {
        var day = week.GetDay(DateTime.Now.DayOfWeek);

        MessageManager.SetHeaderCaption($"Сегодня {week.WeekName}, {day.DayOfWeek.ToString()}");

        foreach (var task in day.tasks)
        {
            var taskItem = _panelManager.CreatePanel<TaskItem>(_panelManager.taskConatainer.transform);

            taskItem.Construct(task);
        }
    }

    public void CheckTaskToShow(TaskInfo task)
    {
        if(task._weekName == GetCurrentWeek().WeekName && task._dayOfWeek == DateTime.Now.DayOfWeek)
        {
            var taskItem = _panelManager.CreatePanel<TaskItem>(_panelManager.taskConatainer.transform);

            taskItem.Construct(task);
        }
    }
}