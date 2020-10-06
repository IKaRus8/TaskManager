using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button createWeekButton;
    public Button createTaskButton;
    public Toggle weekToggle;
    public GameObject weekListContainer;

    private PanelManager _panelManager => PanelManager.Instance;
    private TaskManager _taskManager => TaskManager.Instance;
    private WeekManager _weekManager => WeekManager.Instance;

    private int weekCount;

    private void Awake()
    {
        createWeekButton.onClick.AddListener(_weekManager.WeekDialogConstruct);
        createTaskButton.onClick.AddListener(ShowTaskCreatePanel);
        weekToggle.onValueChanged.AddListener(OnWeekToggleChange);
    }

    private void OnGUI()
    {
        if(_weekManager?.Weeks?.Count != weekCount)
        {
            ClearWeekList();
            FillWeekList();
        }
    }

    private void OnEnable()
    {
        FillWeekList();
    }

    private void OnDisable()
    {
        ClearWeekList();
    }

    private void FillWeekList()
    {
        _weekManager.Weeks.ForEach(w =>
        {
            WeekListItem item = _panelManager.CreatePanel<WeekListItem>(weekListContainer.transform);

            if (item != null)
            {
                item.Init(w);
            }

            weekCount++;
        });
    }

    private void ClearWeekList()
    {
        var weeks = weekListContainer.GetComponentsInChildren<WeekListItem>();

        if (weeks != null && weeks.Any())
        {
            foreach (var t in weeks)
            {
                Destroy(t.gameObject);
            }
        }

        weekCount = 0;
    }

    public void ShowTaskCreatePanel()
    {
        _panelManager.SwitchOffPanels();
        _panelManager.EnableBackground(true);

        _taskManager._taskCreatePanel = _panelManager.CreatePanel<TaskCreatePanel>(_panelManager.panelBack.transform);
        _taskManager._taskCreatePanel.Show();
    }

    private void OnWeekToggleChange(bool value)
    {
        weekListContainer?.SetActive(value);
    }
}
