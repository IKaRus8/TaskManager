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
    public int CurrentValue { get; set; }

    protected List<DateToggle> toggles;
    protected DateTimeFormatInfo DateInfo { get; set; }

    public virtual void Awake()
    {
        toggles = new List<DateToggle>();

        DateInfo = CultureInfo.GetCultureInfo("ru-Ru").DateTimeFormat;
    }

    public virtual void AddToggle(int value)
    {
        DateToggle newToggle = Instantiate(togglePrefab, parentGo.transform);

        newToggle.Construct(value, Callback);

        if(value == CurrentValue)
        {
            newToggle.SetCurrentDateState();
            newToggle.SetSelectedState();
        }

        toggles.Add(newToggle);
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
}
