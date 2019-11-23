using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Archive
{
    public List<BaseTask> deletedTasks = new List<BaseTask>();

    public void Restore(BaseTask task)
    {

    }

    public void FullRemove(BaseTask task)
    {
        if (!deletedTasks.Remove(task))
        {
            var findTask = deletedTasks.FirstOrDefault(t => t.description == task.description);

            if (findTask != null)
            {
                deletedTasks.Remove(findTask);
            }
        }
    }
}
