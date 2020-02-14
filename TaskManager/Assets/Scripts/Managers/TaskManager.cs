using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<WeekController> Weeks { get; set; }

    //public string storagePath;

    private DialogPanel weekCreateDialog;
    private TaskCreatePanel taskCreatePanel;

    private StorageManager Storage { get; set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Weeks = new List<WeekController>();

        //Storage = new StorageManager(storagePath);
    }

    private void Start()
    {
        Load();

        weekCreateDialog = PanelManager.Instance.GetPanel<DialogPanel>();
        taskCreatePanel = PanelManager.Instance.GetPanel<TaskCreatePanel>();
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
        Storage.Load();
    }

    public void WeekDialogConstruct()
    {
        if (weekCreateDialog != null && weekCreateDialog != default)
        {
            weekCreateDialog.input.text = $"Неделя {Weeks.Count + 1}";
            weekCreateDialog.Action = OnWeekCreate;

            PanelManager.Instance.SwitchOffPanels();
            PanelManager.Instance.EnableBackground(true);
            weekCreateDialog.Show();
        }
    }

    private void OnWeekCreate(string text)
    {
        AddWeek(text);
    }

    public void ShowTaskCreatePanel()
    {
        taskCreatePanel.Show();
    }
}

public enum TaskType
{
    Recurring,
    Disposable,
    Deleted
}
