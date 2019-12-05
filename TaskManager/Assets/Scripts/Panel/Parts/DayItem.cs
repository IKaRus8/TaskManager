using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayItem : MonoBehaviour
{
    public DayOfWeek day;
    public Button button;
    public Text text;

    public WeekController Week { get; set; }
}
