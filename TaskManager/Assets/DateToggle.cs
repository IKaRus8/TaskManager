using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateToggle : MonoBehaviour
{
    public Toggle toggle;
    public Image background;
    public Color currentDateColor;
    public Color SelectedDateColor;
    public int Value { get; set; }

    private Color DefaultColor { get; set; }

    private void Awake()
    {
        DefaultColor = background.color;
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void SetCurrentDateState()
    {
        background.color = currentDateColor;
    }

    public void SetSelectedState()
    {
        background.color = SelectedDateColor;
    }

    private void SetDefaultState()
    {
        background.color = DefaultColor;
    }

    private void OnToggleValueChanged(bool value)
    {
        if (value)
        {
            SetSelectedState();
        }
        else
        {
            SetDefaultState();
        }
    }
}
