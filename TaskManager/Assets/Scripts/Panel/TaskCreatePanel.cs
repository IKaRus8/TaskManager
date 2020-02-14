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

    [Space]
    public DayItem dayWeekItemGo;
    public Button createButton;

    [Space]
    public InputField description;
    public GameObject dayGo;
    public GameObject weekGo;

    private TaskManager TaskManager { get; set; }
    private WeekController Week { get; set; }
    private DayController Day { get; set; }
    private List<DayItem> Days { get; set; }

    protected override void Awake()
    {
        base.Awake();

        createButton.onClick.AddListener(Create);

        Days = dayGo.GetComponentsInChildren<DayItem>().ToList();
    }

    protected override void Start()
    {
        base.Start();

        TaskManager = TaskManager.Instance;

        weekGo.SetActive(false);
        dayGo.SetActive(false);

        Construct();
    }

    public void Construct()
    {
        TaskManager.Weeks.ForEach(w => 
        {
            DayItem controller = Instantiate(dayWeekItemGo, weekGo.transform);

            controller.Week = w;
            controller.text.text = w.WeekName;
            controller.button.onClick.AddListener(() => OnWeekButtonClick(controller));
        });

        Days.ForEach(d => d.button.onClick.AddListener(() => OnDayButtonClick(d)));

        Week = TaskManager.Weeks?.FirstOrDefault();
        Day = Week?.days?.FirstOrDefault();
    }

    private void Create()
    {
        BaseTask newtask;

        if (RecurringType)
        {
            newtask = new TaskItem();
        }
        else
        {
            newtask = new BaseTask();
        }

        newtask.Construct(description.text);

        Week.AddTask(Day, newtask);
    }

    private void OnWeekButtonClick(DayItem item)
    {
        Week = item.Week;

        weekChangeButton.GetComponentInChildren<Text>().text = Week.WeekName;

        weekGo.gameObject.SetActive(false);
    }

    private void OnDayButtonClick(DayItem item)
    {
        Day = Week.days.FirstOrDefault(d => d.DayOfWeek == item.day);

        dayChangeButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(DayOfWeek), Day.DayOfWeek);

        dayGo.gameObject.SetActive(false);
    }
}
