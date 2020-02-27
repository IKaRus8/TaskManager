using DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        return MongoDbAtlasManager.GetTasks();
    }

    public static void Update(WeekController week)
    {
        MongoDbAtlasManager.UpdateUser(week);
    }

    public static void Remove()
    {

    }
}
