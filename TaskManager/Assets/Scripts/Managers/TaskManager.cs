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
        taskCreatePanel = _panelManager.GetPanel<TaskCreatePanel>();
    }

    public void CreateTask()
    {

    }

    public void AddWeek(string weekName)
    {
        WeekController newWeek = new WeekController(Weeks.Count + 1, weekName);

        Weeks.Add(newWeek);

        Save();
    }

    public void Save()
    {
        Storage.Save();
    }

    public void Load()
    {
        var tasks = Storage.Load();

        foreach(var task in tasks)
        {

        }
    }

    public void Update()
    {
        Storage.Update();
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
        taskCreatePanel.Show();
    }
}
