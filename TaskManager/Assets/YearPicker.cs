using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearPicker : DatePicker
{
    public override void Awake()
    {
        base.Awake();

        CurrentValue = DateTime.Now.Year;

        gameObject.SetActive(false);
    }
}
