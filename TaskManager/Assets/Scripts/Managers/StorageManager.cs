
using DataBase;
using System.Collections.Generic;

public class StorageManager
{
    public void Save(WeekController week)
    {

    }

    public void Save(TaskInfo task)
    {
        MongoDbAtlasManager.AddTask(task);
    }

    public void Save()
    {

    }

    public List<TaskInfo> Load()
    {
        return MongoDbAtlasManager.GetTasks();
    }

    public void Update()
    {

    }
}
