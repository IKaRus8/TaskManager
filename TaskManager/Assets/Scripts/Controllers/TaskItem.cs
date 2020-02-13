using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : BaseTask
{
    public Toggle doneToggle;
    public Color doneColor;

    public string _weekname;
    public DayOfWeek _dayOfWeek;

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

        _weekname = weekName;
        _dayOfWeek = dayOfWeek;
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
