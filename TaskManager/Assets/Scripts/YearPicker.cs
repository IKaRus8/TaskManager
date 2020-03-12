using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearPicker : DatePicker
{
    public override void Awake()
    {
        base.Awake();

        gameObject.SetActive(false);
    }

    public override void AddToggle(DateTime date, int value)
    {
        DateToggle newToggle = Instantiate(togglePrefab, parentGo.transform);

        newToggle.Construct(value, Callback);

        if (value == CurrentValue.Year)
        {
            newToggle.SetCurrentDateState();
            //newToggle.SetSelectedState();
        }

        toggles.Add(newToggle);
    }
}
