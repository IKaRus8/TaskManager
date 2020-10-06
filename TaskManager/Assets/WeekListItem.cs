using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeekListItem : MonoBehaviour, ITempPanel
{
    public Button showInfoButton;
    public Button removeButton;
    public Text label;

    private WeekController _week;

    private PanelManager _panelManager => PanelManager.Instance;

    private void Awake()
    {
        showInfoButton.onClick.AddListener(OnShowInfoButtonClick);
        removeButton.onClick.AddListener(OnRemoveButtonClick);
    }

    public void Init(WeekController week)
    {
        _week = week;

        label.text = week.WeekName;
    }

    private void OnShowInfoButtonClick()
    {
        var panel = _panelManager.CreatePanel<WeekInfoPanel>(_panelManager.panelBack.transform);

        if(panel != null)
        {
            _panelManager.EnableBackground(true);

            panel.Init(_week);
        }
    }

    private void OnRemoveButtonClick()
    {
        Close();
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
