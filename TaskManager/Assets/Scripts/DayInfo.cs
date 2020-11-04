using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DayInfo : MonoBehaviour
{
    public RectTransform taskContainer;
    public Image background;
    public Text capture;
    public Color activeColor;

    private Toggle toggle;

    private bool haveTask;

    [Inject]
    private PanelManager _panelManager;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(OnToggleChange);
    }

    public void Init(List<TaskInfo> tasks)
    {
        //capture.text = text;

        if(tasks != null && tasks.Any())
        {
            haveTask = true;
            capture.text += $" ({tasks.Count})";
            background.color = activeColor;

            tasks.ForEach(t =>
            {
                var taskItem = _panelManager.CreatePanel<BaseTask>(taskContainer);

                taskItem.Init(t);
            });
        }
    }

    private void OnToggleChange(bool value)
    {
        if (haveTask)
        {
            taskContainer.gameObject.SetActive(value);
        }
    }
}
