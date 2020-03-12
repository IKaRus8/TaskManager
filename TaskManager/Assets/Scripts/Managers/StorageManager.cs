using DataBase;
using System.Collections.Generic;

public static class StorageManager
{
    public static void Save()
    {

    }

    public static void Save(TaskInfo task)
    {
        MongoDbAtlasManager.AddTask(task);
    }

    public static List<TaskInfo> Load()
    {
        return MongoDbAtlasManager.GetTasksByUser();
    }

    public static void Update(WeekController week)
    {
        MongoDbAtlasManager.UpdateUser(week);
    }

    public static void Update(TaskInfo task)
    {
        MongoDbAtlasManager.UpdateTask(task);
    }

    public static void Remove()
    {

    }
}
