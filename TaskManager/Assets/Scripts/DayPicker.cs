using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DayPicker : DatePicker
{
    public DayOfWeek DayName { get; set; }

    private bool first = true;

    public override void Awake()
    {
        base.Awake();

        CurrentValue = DateTime.Now.Day;

        toggles = parentGo.GetComponentsInChildren<DateToggle>().ToList();
    }

    public void AddToggle(DateTime dateTime)
    {
        if (first)
        {
            first = false;

            DayOfWeek dayOfWeek = DateInfo.Calendar.GetDayOfWeek(dateTime);

            if(dayOfWeek != DayOfWeek.Sunday)
            {
                toggles.Take((int)dayOfWeek - 1).ToList().ForEach(t => t.SetDisableState());
            }
            else
            {
                toggles.Take(6).ToList().ForEach(t => t.SetDisableState());
            }
        }
        else
        {
            DateToggle toggle = toggles.FirstOrDefault(t => t.state != DateToggleState.Disable && t.Value == 0);

            if (toggle != null)
            {
                toggle.Construct(dateTime.Day, Callback);

                if (dateTime.Day == CurrentValue)
                {
                    toggle.SetCurrentDateState();
                    toggle.SetSelectedState();
                }
            }
        }
    }

    public void SetOtherDayDisable()
    {
        toggles.Where(t => t.state == DateToggleState.Active && t.Value == 0).ToList().ForEach(t => t.SetDisableState());
    }
}
