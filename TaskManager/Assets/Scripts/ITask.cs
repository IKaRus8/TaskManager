using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITask
{
    void Create(string text);
    void OnToggleValueChanged(bool value);
}
