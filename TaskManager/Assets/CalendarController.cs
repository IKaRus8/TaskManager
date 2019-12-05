using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CalendarController : MonoBehaviour
{
    public DatePicker yearPicker;
    public DatePicker monthPicker;
    public DatePicker dayPicker;

    public GameObject togglePrefab;

    private DateTime SelectedDate { get; set; }
    private DateTime CurrentDate { get; set; }

    private DateTimeFormatInfo DateInfo { get; set; }

    private int AvailableYears = 3;

    private void Awake()
    {
        SelectedDate = new DateTime();
        CurrentDate = DateTime.Now;
        DateInfo = CultureInfo.GetCultureInfo("ru-Ru").DateTimeFormat;

        yearPicker.Callback = OnYearToggleClick;
        monthPicker.Callback = OnMonthToggleClick;
        dayPicker.Callback = OnDayToggleClick;
    }

    private void FillYear()
    {
        for( int i = CurrentDate.Year; i <= CurrentDate.Year + AvailableYears; i++)
        {
            yearPicker.AddToggle(i);
        }
    }

    private void FillMonth()
    {
        for(int i = 1; i <= DateInfo.MonthNames.Length; i++)
        {
            monthPicker.AddToggle(i);
        }
    }

    private void FillDays()
    {
        int dayCount = DateInfo.Calendar.GetDaysInMonth(SelectedDate.Year, SelectedDate.Month);

        for (var i = 1; i <= dayCount; i++)
        {
            dayPicker.AddToggle(i);
        }
    }

    private void OnYearToggleClick(int year)
    {
        SelectedDate = new DateTime(year, SelectedDate.Month, SelectedDate.Day);
    }

    private void OnMonthToggleClick(int month)
    {
        SelectedDate = new DateTime(SelectedDate.Year, month, SelectedDate.Day);
    }

    private void OnDayToggleClick(int day)
    {
        SelectedDate = new DateTime(SelectedDate.Year, SelectedDate.Month, day);
    }
}
