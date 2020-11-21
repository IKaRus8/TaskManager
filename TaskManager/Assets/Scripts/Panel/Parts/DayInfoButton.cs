using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DayInfoButton : MonoBehaviour
{
    //public DayOfWeek day;
    private Button button;
    //public Text text;

    public DayInfo Info { get; private set; }
    public WeekController Week { get; private set; }

    public void Init(WeekController week, UnityAction<DayInfoButton> callback)
    {
        BaseInit(callback);

        Week = week;

        Info.capture.text = week.WeekName;
    }

    public void Init(DayOfWeek dayOfWeek, UnityAction<DayInfoButton> callback)
    {
        BaseInit(callback);

        Info.Init(dayOfWeek);
    }

    private void BaseInit(UnityAction<DayInfoButton> callback)
    {
        Info = GetComponent<DayInfo>();

        button = gameObject.AddComponent<Button>();

        button.onClick.AddListener(() => callback?.Invoke(this));
    }
}
