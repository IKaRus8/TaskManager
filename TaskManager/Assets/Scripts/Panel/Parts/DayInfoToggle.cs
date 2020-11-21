using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Panel.Parts
{
    public class DayInfoToggle : MonoBehaviour
    {
        private DayInfo dayInfo;

        private Toggle toggle;

        private bool haveTask;

        private void Awake()
        {
            dayInfo = GetComponent<DayInfo>();
            toggle = gameObject.AddComponent<Toggle>();
        }

        private void OnToggleChange(bool value)
        {
            if (haveTask)
            {
                dayInfo.taskContainer.gameObject.SetActive(value);
            }
        }

        public void Init(List<TaskInfo> tasks, DayOfWeek day)
        {
            dayInfo.Init(day);
            toggle?.onValueChanged.AddListener(OnToggleChange);

            if (tasks != null && tasks.Any())
            {
                haveTask = true;
                dayInfo.capture.text += $" ({tasks.Count})";
                dayInfo.background.color = dayInfo.activeColor;

                tasks.ForEach(t =>
                {
                    var taskItem = dayInfo._panelManager.CreatePanel<BaseTask>(dayInfo.taskContainer);

                    taskItem.Init(t);
                });
            }
        }
    }
}
