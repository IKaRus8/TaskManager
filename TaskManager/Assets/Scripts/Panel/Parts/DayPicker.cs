using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DayPicker : DatePicker
{
    private bool first = true;

    public DateTime selectedDate;

    public override void Awake()
    {
        base.Awake();

        toggles = parentGo.GetComponentsInChildren<DateToggle>().ToList();
    }

    public override void AddToggle(DateTime date, int value)
    {
        if (first)
        {
            first = false;

            DayOfWeek dayOfWeek = DateInfo.Calendar.GetDayOfWeek(date);

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
                toggle.Construct(value, Callback);

                if (date.ToShortDateString() == CurrentValue.ToShortDateString())
                {
                    toggle.SetCurrentDateState();
                    //toggle.SetSelectedState();
                }
            }
        }
    }

    public void SetOtherDayDisable()
    {
        toggles.Where(t => t.state == DateToggleState.Active && t.Value == 0).ToList().ForEach(t => t.SetDisableState());
    }

    public void DisableOtherDayOfWeek(List<DayOfWeek> activeDays)
    {
        toggles.Where(t =>
        {
            if (t.state == DateToggleState.Active)
            {
                var tempDate = new DateTime(selectedDate.Year, selectedDate.Month, t.Value);

                var dayOfWeek = DateInfo.Calendar.GetDayOfWeek(tempDate);

                return !activeDays.Contains(dayOfWeek); 
            }
            else
            {
                return false;
            }
            
        }).ToList().ForEach(t => t.SetDisableState());
    }

    public void Clear()
    {
        toggles.ForEach(t => t.SetDefaultState());
        first = true;
    }
}
