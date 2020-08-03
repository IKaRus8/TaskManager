using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskCreatePanel : BasePanel, ITempPanel
{
    public Toggle taskType;
    public Toggle inAllWeek;
    public Button weekChangeButton;
    public Button dayChangeButton;
    public Button CalendarButton;
    public Button createButton;
    public Button closeButton;

    public CalendarPanel calendar;

    [Space]
    public DayItem dayWeekItemGo;

    [Space]
    public InputField nameInputField;
    public InputField descriptionInputField;
    public GameObject dayGo;
    public GameObject weekParentGo;

    private TaskManager _taskManager => TaskManager.Instance;
    private PanelManager _panelManager => PanelManager.Instance;
    private WeekManager _weekManager => WeekManager.Instance;

    private WeekController Week { get; set; }
    private DayOfWeek Day { get; set; }
    private List<DayItem> Days { get; set; }
    private DateTime TaskDate { get; set; }

    protected override void Awake()
    {
        base.Awake();

        createButton.onClick.AddListener(OnCreateButtonClick);
        closeButton.onClick.AddListener(Close);

        taskType.onValueChanged.AddListener(OnTypeToggleChanged);
        inAllWeek.onValueChanged.AddListener(OnAllWeekToggleChanged);
        CalendarButton.onClick.AddListener(OnCalendarButtonClick);
        calendar.Callback = OnDaySelected;

        Days = dayGo.GetComponentsInChildren<DayItem>().ToList();
    }

    protected override void Start()
    {
        base.Start();

        weekParentGo.SetActive(false);
        dayGo.SetActive(false);

        Construct();
    }

    public void Construct()
    {
        _weekManager.Weeks.ForEach(w => 
        {
            DayItem controller = Instantiate(dayWeekItemGo, weekParentGo.transform);

            controller.Week = w;
            controller.text.text = w.WeekName;
            controller.button.onClick.AddListener(() => OnWeekButtonClick(controller));
        });

        Week = _weekManager.Weeks?.FirstOrDefault();
        Day = DayOfWeek.Monday;

        Days.ForEach(d => d.button.onClick.AddListener(() => OnDayButtonClick(d)));
    }

    //TODO: обработать исключения
    private void OnCreateButtonClick()
    {
        if(nameInputField.text == "")
        {
            nameInputField.placeholder.color = Color.red;

            return;
        }

        if (inAllWeek)
        {
            _weekManager.Weeks.ForEach(w => Create(w));
        }
        else
        {
            Create(Week);
        }

        Close();
    }

    private void Create(WeekController week)
    {
        TaskInfo newTask = new TaskInfo
        {
            isRecurring = taskType.isOn,
            _name = nameInputField.text,
            _descriptionText = descriptionInputField.text,
            _weekName = week.WeekName,
            _dayOfWeek = Day,
            _date = TaskDate
        };

        _taskManager.Create(Week, newTask);
    }

    private void OnWeekButtonClick(DayItem item)
    {
        Week = item.Week;

        weekChangeButton.GetComponentInChildren<Text>().text = Week.WeekName;

        weekParentGo.gameObject.SetActive(false);
    }

    private void OnDayButtonClick(DayItem item)
    {
        Day = item.day;

        dayChangeButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(DayOfWeek), Day);

        dayGo.gameObject.SetActive(false);
    }

    private void OnCalendarButtonClick()
    {
        calendar.Show();
    }

    private void OnDaySelected(DateTime date)
    {
        var dateText = CalendarButton.GetComponentInChildren<Text>();
        if(dateText != null)
        {
            dateText.text = date.ToString();//"Yy:Mm:Dd");
        }

        TaskDate = date;

        calendar.Close();
    }

    private void OnTypeToggleChanged(bool value)
    {
        weekChangeButton.gameObject.SetActive(value);
        dayChangeButton.gameObject.SetActive(value);

        CalendarButton.gameObject.SetActive(!value);
    }

    private void OnAllWeekToggleChanged(bool value)
    {
        weekChangeButton.gameObject.SetActive(!value);
    }

    public override void Close()
    {
        _panelManager.EnableBackground(false);

        Destroy(gameObject);
    }
}