using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<WeekController> Weeks { get; set; }

    public string storagePath;

    public DialogController weekCreateDialog;

    private StorageManager Storage { get; set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Weeks = new List<WeekController>();

        Storage = new StorageManager(storagePath);
    }

    private void Start()
    {
        Load();
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
        weekCreateDialog.input.text = $"Неделя {Weeks.Count + 1}";
        weekCreateDialog.Action = OnWeekCreate;

        weekCreateDialog.gameObject.SetActive(true);
    }

    private void OnWeekCreate(string text)
    {
        AddWeek(text);
    }
}

public enum TaskType
{
    Recurring,
    Disposable,
    Deleted
}
