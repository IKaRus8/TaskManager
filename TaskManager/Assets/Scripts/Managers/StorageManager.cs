
using DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StorageManager
{
    public void Save()
    {

    }

    public void Save(TaskInfo task)
    {
        MongoDbAtlasManager.AddTask(task);
    }

    public List<TaskInfo> Load()
    {
        return MongoDbAtlasManager.GetTasks();
    }

    public void Update(WeekController week)
    {
        MongoDbAtlasManager.UpdateUser(week);
    }

    public void Remove()
    {

    }
}
