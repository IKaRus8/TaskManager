using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskCreatePanel : BasePanel
{
    public Toggle RecurringType;
    public Button weekChangeButton;
    public Button dayChangeButton;
    public Button createButton;

    [Space]
    public DayItem dayWeekItemGo;

    [Space]
    public InputField description;
    public GameObject dayGo;
    public GameObject weekGo;

    private TaskManager _taskManager => TaskManager.Instance;
    private WeekController Week { get; set; }
    private DayOfWeek Day { get; set; }
    private List<DayItem> Days { get; set; }

    private StorageManager Storage = new StorageManager();

    protected override void Awake()
    {
        base.Awake();

        createButton.onClick.AddListener(Create);

        Days = dayGo.GetComponentsInChildren<DayItem>().ToList();
    }

    protected override void Start()
    {
        base.Start();

        weekGo.SetActive(false);
        dayGo.SetActive(false);

        Construct();
    }

    public void Construct()
    {
        _taskManager.Weeks.ForEach(w => 
        {
            DayItem controller = Instantiate(dayWeekItemGo, weekGo.transform);

            controller.Week = w;
            controller.text.text = w.WeekName;
            controller.button.onClick.AddListener(() => OnWeekButtonClick(controller));
        });

        Days.ForEach(d => d.button.onClick.AddListener(() => OnDayButtonClick(d)));

        Week = _taskManager.Weeks?.FirstOrDefault();
    }

    private void Create()
    {
        TaskInfo newtask = new TaskInfo
        {
            isRecurring = RecurringType.isOn,
            _descriptionText = description.text,
            _weekname = Week.WeekName,
            _dayOfWeek = Day
        };

        Week.AddTask(newtask);

        Storage.Save(newtask);
    }

    private void OnWeekButtonClick(DayItem item)
    {
        Week = item.Week;

        weekChangeButton.GetComponentInChildren<Text>().text = Week.WeekName;

        weekGo.gameObject.SetActive(false);
    }

    private void OnDayButtonClick(DayItem item)
    {
        Day = item.day;

        dayChangeButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(DayOfWeek), Day);

        dayGo.gameObject.SetActive(false);
    }
}
