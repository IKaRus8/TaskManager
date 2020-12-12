namespace Assets.Scripts.DI.Signals
{
    public class TaskRemovedSignal
    {
        public TaskInfo Task { get; set; }

        public TaskRemovedSignal(TaskInfo task)
        {
            Task = task;
        }
    }
}
