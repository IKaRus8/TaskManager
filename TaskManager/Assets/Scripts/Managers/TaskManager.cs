using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<WeekController> weeks;

    public string storagePath;

    private StorageManager Storage { get; set; }

    private void Awake()
    {
        weeks = new List<WeekController>();

        Storage = new StorageManager(storagePath);
    }

    private void Start()
    {
        Load();
    }

    public void CreateTask()
    {

    }

    public void AddWeek()
    {
        
    }

    public void Save()
    {
        Storage.Save();
    }

    public void Load()
    {
        Storage.Load();
    }
}

public enum TaskType
{
    Recurring,
    Disposable,
    Deleted
}
