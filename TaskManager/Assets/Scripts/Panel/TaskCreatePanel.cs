using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskCreatePanel : BasePanel, ITempPanel
{
    public Toggle taskType;
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
    private WeekController Week { get; set; }
    private DayOfWeek Day { get; set; }
    private List<DayItem> Days { get; set; }
    private DateTime TaskDate { get; set; }

    protected override void Awake()
    {
        base.Awake();

        createButton.onClick.AddListener(Create);
        closeButton.onClick.AddListener(Close);

        taskType.onValueChanged.AddListener(OnTypeToggleChanged);
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
        _taskManager.Weeks.ForEach(w => 
        {
            DayItem controller = Instantiate(dayWeekItemGo, weekParentGo.transform);

            controller.Week = w;
            controller.text.text = w.WeekName;
            controller.button.onClick.AddListener(() => OnWeekButtonClick(controller));
        });

        Week = _taskManager.Weeks?.FirstOrDefault();
        Day = DayOfWeek.Monday;

        Days.ForEach(d => d.button.onClick.AddListener(() => OnDayButtonClick(d)));
    }

    //TODO: обработать исключения
    private void Create()
    {
        if(nameInputField.text == "")
        {
            nameInputField.placeholder.color = Color.red;

            return;
        }

        TaskInfo newTask = new TaskInfo
        {
            isRecurring = taskType.isOn,
            _name = nameInputField.text,
            _descriptionText = descriptionInputField.text,
            _weekName = Week.WeekName,
            _dayOfWeek = Day,
            _date = TaskDate
        };

        if (newTask.isRecurring)
        {
            Week.AddTask(newTask); 
        }
        else
        {

        }

        _taskManager.CheckTaskToShow(newTask);

        StorageManager.Save(newTask);

        Close();
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

    public override void Close()
    {
        _panelManager.EnableBackground(false);

        Destroy(gameObject);
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
}