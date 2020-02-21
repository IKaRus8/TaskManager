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

        Weeks.Add(newWeek);

        Storage.Update(newWeek);
    }

    public void Save()
    {
        Storage.Save();
    }

    public void OnAuthorization(List<WeekController> weeks)
    {
        weeks.ForEach(w => AddWeek(w));

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
    }

    public void ShowTaskCreatePanel()
    {
        _panelManager.SwitchOffPanels();
        _panelManager.EnableBackground(true);

        taskCreatePanel = _panelManager.CreatePanel<TaskCreatePanel>();
        taskCreatePanel.Show();
    }
}
