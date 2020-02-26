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

    public void ActiveOneDayOfWeek()
    {
        System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;

        DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
        DayOfWeek today = DateTime.Now.DayOfWeek;
        DateTime sow = DateTime.Now.AddDays(-(today - fdow)).Date;
    }
}
