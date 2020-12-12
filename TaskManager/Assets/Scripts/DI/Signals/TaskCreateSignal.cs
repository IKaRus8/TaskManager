using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DI.Signals
{
    public class TaskCreateSignal
    {
        public WeekController Week { get; set; }
        public TaskInfo Info { get; set; }

        public TaskCreateSignal(WeekController week, TaskInfo info)
        {
            Week = week;
            Info = info;
        }
    }
}