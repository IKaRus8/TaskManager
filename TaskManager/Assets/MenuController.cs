using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button createWeekButton;
    public Button createTaskButton;

    private PanelManager _panelManager => PanelManager.Instance;
    private TaskManager _taskManager => TaskManager.Instance;
    private WeekManager _weekManager => WeekManager.Instance;

    private void Awake()
    {
        createWeekButton.onClick.AddListener(_weekManager.WeekDialogConstruct);
        createTaskButton.onClick.AddListener(ShowTaskCreatePanel);
    }

    public void ShowTaskCreatePanel()
    {
        _panelManager.SwitchOffPanels();
        _panelManager.EnableBackground(true);

        _taskManager._taskCreatePanel = _panelManager.CreatePanel<TaskCreatePanel>(_panelManager.panelBack.transform);
        _taskManager._taskCreatePanel.Show();
    }
}
