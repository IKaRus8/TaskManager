using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DI.Signals
{
    public class SetActiveTutorialSignal
    {
        public bool Value { get; set; }

        public SetActiveTutorialSignal(bool value)
        {
            Value = value;
        }
    }
}
