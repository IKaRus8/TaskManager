using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : BaseTask
{
    public Toggle doneToggle;
    public Color doneColor;

    override public void Awake()
    {
        base.Awake();

        doneToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    override public void Construct(string text)
    {
        base.Construct(text);
    }

    override protected void OnToggleValueChanged(bool value)
    {
        base.OnToggleValueChanged(value);

        if (IsDone)
        {
            background.color = doneColor;
        }
        else
        {
            background.color = defColor;
        }
    }
}
