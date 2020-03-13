using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DateToggle : MonoBehaviour
{
    public Toggle toggle;
    public Image background;
    public Text label;
    public Color currentDateColor;
    public Color disableColor;
    public DateToggleState state;
    public int Value { get; set; }
    private UnityAction<int> Callback { get; set; }
    private Color defColor;

    private void Awake()
    {
        defColor = background.color;
    }

    public void Construct(int value, UnityAction<int> callback)
    {
        Value = value;
        Callback = callback;
        label.text = value.ToString();

        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        state = DateToggleState.Active;
    }

    public void SetCurrentDateState()
    {
        background.color = currentDateColor;
    }

    public void SetSelectedState()
    {
        toggle.isOn = true;
    }

    public void SetDefaultState()
    {
        state = DateToggleState.Active;
        background.color = defColor;
        label.text = "0";
        Value = 0;
    }

    public void SetDisableState()
    {
        toggle.interactable = false;
        state = DateToggleState.Disable;
        background.color = disableColor;
    }

    private void OnToggleValueChanged(bool value)
    {
        if (value)
        {
            Callback?.Invoke(Value);
            state = DateToggleState.Selected;
        }
        else
        {
            state = DateToggleState.Active;
        }
    }
}

public enum DateToggleState
{
    Active,
    Selected,
    Disable
}
