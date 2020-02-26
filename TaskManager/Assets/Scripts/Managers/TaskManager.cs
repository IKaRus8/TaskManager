using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<WeekController> Weeks { get; set; }

    private DialogPanel weekCreateDialog;
    private TaskCreatePanel taskCreatePanel;

    private StorageManager Storage = new StorageManager();

    private PanelManager _panelManager => PanelManager.Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Weeks = new List<WeekController>();
    }

    private void Start()
    {
        weekCreateDialog = _panelManager.GetPanel<DialogPanel>();
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
            newWeek.startWeek = GetNextWeekDay(Weeks.Last().startWeek.AddDays(1));
        }
        else
        {
            DateTime input = DateTime.Now;
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            newWeek.startWeek = input.AddDays(delta);
        }

        Weeks.Add(newWeek);

        Storage.Update(newWeek);
    }

    public void Save()
    {
        Storage.Save();
    }

    public void OnAuthorization(List<WeekController> weeks)
    {
        weeks?.OrderBy(w => w?.startWeek)?.ToList().ForEach(w => AddWeek(w));

        LoadTasks();
    }

    public void LoadTasks()
    {
        var tasks = Storage.Load();

        foreach(var task in tasks)
        {
            var week = Weeks.FirstOrDefault(w => w.WeekName == task._weekName);

            if (week != null)
            {
                week.AddTask(task);
            }
        }

        GetCurrentDay();
    }

    public void WeekDialogConstruct()
    {
        if (weekCreateDialog != null && weekCreateDialog != default)
        {
            weekCreateDialog.input.text = $"Неделя {Weeks.Count + 1}";
            weekCreateDialog.Action = OnWeekCreate;

            _panelManager.SwitchOffPanels();
            _panelManager.EnableBackground(true);
            weekCreateDialog.Show();
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

    public static DateTime GetNextWeekDay(DateTime start, DayOfWeek day = DayOfWeek.Monday)
    {
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
        return start.AddDays(daysToAdd);
    }

    public void GetCurrentDay()
    {
        var week = Weeks.Where(w => w.startWeek <= DateTime.Now).Max();

        if(week.startWeek.AddDays(7) < DateTime.Now)
        {
            var firstWeek = Weeks.FirstOrDefault(w => w.startWeek == Weeks.Min(m => m.startWeek));

            firstWeek.startWeek = GetNextWeekDay(week.startWeek);

            GetCurrentDay();
        }

        SetTodayValue(week);
    }

    public void SetTodayValue(WeekController week)
    {
        var day = week.GetDay(DateTime.Now.DayOfWeek);

        MessageManager.SetHeaderCaption($"Сегодня {week.WeekName}, {day.DayOfWeek.ToString()}");

        foreach (var task in day.tasks)
        {
            var taskItem = _panelManager.CreatePanel<TaskItem>(_panelManager.taskConatainer.transform);

            taskItem.taskInfo = task;
            taskItem.SetText();
        }
    }
}