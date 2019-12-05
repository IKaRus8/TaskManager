using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DatePicker : MonoBehaviour
{
    public DateToggle togglePrefab;
    public UnityAction<int> Callback { get; set; }
    public GameObject ParentGo { get; set; }

    private List<DateToggle> toggles;

    public virtual void Awake()
    {
        toggles = new List<DateToggle>();
    }

    public virtual void AddToggle(int value)
    {
        DateToggle newToggle = Instantiate(togglePrefab, ParentGo.transform);

        newToggle.Value = value;
        newToggle.toggle.onValueChanged.AddListener((b) => Callback(value));
    }
}
