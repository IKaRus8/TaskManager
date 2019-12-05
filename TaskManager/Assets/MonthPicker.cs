using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MonthPicker : DatePicker
{
    private string MonthName { get; set; }
    private DateTimeFormatInfo DateInfo { get; set; }

    public override void Awake()
    {
        base.Awake();

        DateInfo = CultureInfo.GetCultureInfo("ru-Ru").DateTimeFormat;
    }

    public override void AddToggle(int value)
    {
        base.AddToggle(value);

        MonthName = DateInfo.GetMonthName(value);
    }
}
