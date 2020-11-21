using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Объект дня недели
/// </summary>
public class DayInfo : BaseTempElement
{
    public RectTransform taskContainer;
    public Image background;
    public Text capture;
    public Color activeColor;

    public DayOfWeek dayOfWeek;

    [Inject]
    [HideInInspector]
    public UIManager _panelManager;

    public string Capture
    {
        get
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dayOfWeek);
        }
    }

    public virtual void Init(DayOfWeek day)
    {
        dayOfWeek = day;

        capture.text = Capture;
    }
}
