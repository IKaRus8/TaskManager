using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CalendarController : BasePanel
{
    public DatePicker yearPicker;
    public DatePicker monthPicker;
    public DayPicker dayPicker;

    public Toggle yearToggle;
    public Toggle MonthToggle;
    public Toggle dayToggle;

    public UnityAction<DateTime> Callback { get; set; }

    private DateTime SelectedDate { get; set; }
    private DateTime CurrentDate { get; set; }

    private DateTimeFormatInfo DateInfo { get; set; }

    private int AvailableYears = 3;

    protected override void Awake()
    {
        SelectedDate = CurrentDate = DateTime.Now;

        DateInfo = CultureInfo.GetCultureInfo("ru-Ru").DateTimeFormat;

        yearPicker.Callback = OnYearSelect;
        monthPicker.Callback = OnMonthSelect;
        dayPicker.Callback = OnDaySelect;

        yearToggle.onValueChanged.AddListener(OnYearToggleChange);
        MonthToggle.onValueChanged.AddListener(OnMonthToggleChange);
        dayToggle.onValueChanged.AddListener(OnDayToggleChange);
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
            yearPicker.AddToggle(SelectedDate, i);
        }
    }

    private void FillMonth()
    {
        DateTime tempDate = SelectedDate;

        for(int i = 0; i < DateInfo.MonthNames.Length; i++)
        {
            monthPicker.AddToggle(tempDate, i);
        }
    }

    private void FillDays()
    {
        dayPicker.Clear();

        int dayCount = DateInfo.Calendar.GetDaysInMonth(SelectedDate.Year, SelectedDate.Month);

        for (var i = 0; i <= dayCount; i++)
        {
            DateTime dateTime = new DateTime(SelectedDate.Year, SelectedDate.Month, Mathf.Max(1, i));

            dayPicker.AddToggle(dateTime, i);
        }

        dayPicker.SetOtherDayDisable();
        //dayPicker.DisableOtherDayOfWeek(new List<DayOfWeek> { DayOfWeek.Monday});
    }

    private void OnYearSelect(int year)
    {
        SelectedDate = new DateTime(year, 1, 1);

        monthPicker.SetDate(1);

        MonthToggle.isOn = true;

        FillDays();
    }

    private void OnMonthSelect(int month)
    {
        SelectedDate = new DateTime(SelectedDate.Year, month, 1);

        dayPicker.selectedDate = SelectedDate;

        FillDays();
        dayToggle.isOn = true;
    }

    private void OnDaySelect(int day)
    {
        SelectedDate = new DateTime(SelectedDate.Year, SelectedDate.Month, day).AddHours(3);

        Callback?.Invoke(SelectedDate);
    }

    private void OnYearToggleChange(bool value)
    {
        yearPicker.ShowHidePicker(value);
    }

    private void OnMonthToggleChange(bool value)
    {
        monthPicker.ShowHidePicker(value);
    }

    private void OnDayToggleChange(bool value)
    {
        dayPicker.ShowHidePicker(value);
    }
}