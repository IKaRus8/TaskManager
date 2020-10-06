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

    public static List<TaskInfo> LoadTasksForUser()
    {
        return MongoDbAtlasManager.GetTasksByUser();
    }

    public static List<TaskInfo> LoadTaskByWeek(string weekName)
    {
        return MongoDbAtlasManager.GetTasksByWeek(weekName);
    }

    public static void Update(WeekController week)
    {
        MongoDbAtlasManager.AddWeek(week);
    }

    public static void Update(TaskInfo task)
    {
        MongoDbAtlasManager.UpdateTask(task);
    }

    public static void Remove(WeekController week)
    {
        List<TaskInfo> tasks = LoadTaskByWeek(week.WeekName);

        tasks.ForEach(t =>
        {
            t.deleted = true;
            Update(t);
        });

        MongoDbAtlasManager.RemoveWeek(week);
    }

    public static void Remove(TaskInfo task)
    {
        task.deleted = true;

        Update(task);
    }
}
