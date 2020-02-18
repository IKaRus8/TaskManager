using System;
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

    public void Construct(string text, string weekName, DayOfWeek dayOfWeek)
    {
        Construct(text);

        taskInfo._weekname = weekName;
        taskInfo._dayOfWeek = dayOfWeek;
    }

    override protected void OnToggleValueChanged(bool value)
    {
        base.OnToggleValueChanged(value);

        if (taskInfo._isDone)
        {
            background.color = doneColor;
        }
        else
        {
            background.color = defColor;
        }
    }
}
