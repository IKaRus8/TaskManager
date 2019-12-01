using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayItem : MonoBehaviour
{
    public DayOfWeek day;

    public WeekController Week { get; set; }
    public Button Button { get; set; } 

    private void Awake()
    {
        Button = GetComponent<Button>();
    }
}
