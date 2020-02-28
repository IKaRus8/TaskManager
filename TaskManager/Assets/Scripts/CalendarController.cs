using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CalendarController : BasePanel
{
    public DatePicker yearPicker;
    public DatePicker monthPicker;
    public DayPicker dayPicker;

    public UnityAction<DateTime> Callback { get; set; }

    private DateTime SelectedDate { get; set; }
    private DateTime CurrentDate { get; set; }

    private DateTimeFormatInfo DateInfo { get; set; }

    private int AvailableYears = 3;

    protected override void Awake()
    {
        SelectedDate = new DateTime();
        CurrentDate = DateTime.Now;
        DateInfo = CultureInfo.GetCultureInfo("ru-Ru").DateTimeFormat;

        yearPicker.Callback = OnYearToggleClick;
        monthPicker.Callback = OnMonthToggleClick;
        dayPicker.Callback = OnDayToggleClick;
    }

    protected override void Start()
    {
        FillYear();
        FillMonth();
        FillDays();
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
        for(int i = 1; i < DateInfo.MonthNames.Length; i++)
        {
            monthPicker.AddToggle(i);
        }
    }

    private void FillDays()
    {
        int dayCount = DateInfo.Calendar.GetDaysInMonth(SelectedDate.Year, SelectedDate.Month);

        for (var i = 0; i <= dayCount; i++)
        {
            DateTime dateTime = new DateTime(SelectedDate.Year, SelectedDate.Month, Mathf.Max(1, i));

            dayPicker.AddToggle(dateTime);
        }

        dayPicker.SetOtherDayDisable();
        //dayPicker.DisableOtherDayOfWeek(new List<DayOfWeek> { DayOfWeek.Monday});
    }

    private void OnYearToggleClick(int year)
    {
        SelectedDate = new DateTime(year, SelectedDate.Month, SelectedDate.Day);
    }

    private void OnMonthToggleClick(int month)
    {
        SelectedDate = new DateTime(SelectedDate.Year, month, SelectedDate.Day);
        dayPicker.selectedDate = SelectedDate;
    }

    private void OnDayToggleClick(int day)
    {
        SelectedDate = new DateTime(SelectedDate.Year, SelectedDate.Month, day);

        Callback?.Invoke(SelectedDate);
    }

    private void SetCurrentDay()
    {

    }
}
