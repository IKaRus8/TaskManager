using System;
using System.Linq;

public class MonthPicker : DatePicker
{
    public string MonthName { get; set; }
    
    public override void Awake()
    {
        base.Awake();
        toggles = parentGo.GetComponentsInChildren<DateToggle>().ToList();

        gameObject.SetActive(false);
    }

    public override void AddToggle(DateTime date, int value)
    {
        MonthName = DateInfo.GetMonthName(value + 1);

        if (value < toggles.Count)
        {
            toggles[value].Construct(value + 1, Callback);
            toggles[value].label.text = MonthName;

            if(date.Month == value + 1)
            {
                toggles[value].SetCurrentDateState();
            }
        }
    }
}