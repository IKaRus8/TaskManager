using Assets.Scripts;
using Assets.Scripts.DI.Signals;
using Assets.Scripts.Panel.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Панель информации о неделе
/// </summary>
public class WeekInfoPanel : BaseTempElement
{
    public RectTransform daysContainer;
    public GameObject dialogGO;
    public Text capture;
    public Button backButton;
    public Button removeButton;

    private WeekController _week;

    private SignalBus _signalBus;
    private UIManager _uiManager;

    [Inject]
    private void Construct(SignalBus signalBus, UIManager uIManager)
    {
        _signalBus = signalBus;
        _uiManager = uIManager;
    }

    protected override void Awake()
    {
        base.Awake();

        backButton.onClick.AddListener(Close);

        removeButton.onClick.AddListener(() =>
        {
            var window = _uiManager.CreatePanel<DialogWindow>(null);

            window.Action = () =>
            {
                _signalBus.Fire(new RemoveWeekSignal(_week));

                Close();
            };
        });
    }

    public void Init(WeekController week)
    {
        _week = week;
        capture.text = week.WeekName;

        List<TaskInfo> tasks = UserInfo.Tasks; 

        tasks = tasks?.Where(t => t._weekName == week.WeekName)?.ToList();

        if (tasks != null && tasks.Any())
        {
            foreach (DayOfWeek item in Enum.GetValues(typeof(DayOfWeek)))
            {
                DayInfo day = _uiManager.CreatePanel<DayInfo>(daysContainer);

                var controller = day.gameObject.AddComponent<DayInfoToggle>();

                List<TaskInfo> tasksOfDay = tasks.Where(t => t._dayOfWeek == item).ToList();

                controller.Init(tasksOfDay, item);
            }
        }
    }

    public override void Close()
    {
        _signalBus.Fire(new EnableBackgroundSignal(false));

        base.Close();
    }
}