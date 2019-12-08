using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MonthPicker : DatePicker
{
    public string MonthName { get; set; }
    
    public override void Awake()
    {
        base.Awake();

        CurrentValue = DateTime.Now.Month;

        gameObject.SetActive(false);
    }

    public override void AddToggle(int value)
    {
        base.AddToggle(value);

        MonthName = DateInfo.GetMonthName(value);
    }
}
