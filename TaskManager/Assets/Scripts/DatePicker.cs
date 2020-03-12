using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DatePicker : MonoBehaviour
{
    public DateToggle togglePrefab;
    public GameObject parentGo;
    public UnityAction<int> Callback { get; set; }
    public DateTime CurrentValue { get; set; }

    protected List<DateToggle> toggles = new List<DateToggle>();
    protected DateTimeFormatInfo DateInfo = CultureInfo.GetCultureInfo("ru-Ru").DateTimeFormat;

    public virtual void Awake()
    {
        CurrentValue = DateTime.Now;
    }

    public virtual void AddToggle(DateTime date, int value)
    {
       
    }

    public List<DateToggle> GetSelectedValues()
    {
        var selectedDates = toggles.Where(t => t.state == DateToggleState.Selected).ToList();

        return selectedDates;
    }

    public void SetDate(int value)
    {
        toggles.FirstOrDefault(t => t.Value == value)?.SetSelectedState();
    }

    public void ShowHidePicker(bool value)
    {
        gameObject.SetActive(value);
    }
}
