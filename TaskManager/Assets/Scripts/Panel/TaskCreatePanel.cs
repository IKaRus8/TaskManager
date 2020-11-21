using Assets.Scripts.DI.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TaskCreatePanel : BaseTempElement
{
    [SerializeField]
    private Toggle taskType;
    [SerializeField]
    private Toggle inAllWeekToggle;
    [SerializeField]
    private Button weekChangeButton;
    [SerializeField]
    private Button dayChangeButton;
    [SerializeField]
    private Button CalendarButton;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Text selectedWeekText;
    [SerializeField]
    private Text selectedDayText;

    public CalendarPanel calendar;

    [Space]
    public DayInfoButton dayWeekItemGo;

    [Space]
    public InputField nameInputField;
    public InputField descriptionInputField;
    public RectTransform daysContainerRect;
    public GameObject weekParentGo;

    private SignalBus _signalBus;
    private TaskService _taskManager;
    private WeekManager _weekManager;
    private UIManager _uiManager;

    private WeekController Week { get; set; }
    private DayOfWeek Day { get; set; }
    //private List<DayInfoButton> Days { get; set; }
    private DateTime TaskDate { get; set; }

    [Inject]
    private void Construct(SignalBus signalBus, TaskService taskManager, WeekManager weekManager, UIManager uiManager)
    {
        _signalBus = signalBus;
        _taskManager = taskManager;
        _weekManager = weekManager;
        _uiManager = uiManager;
    }

    protected override void Awake()
    {
        base.Awake();

        createButton.onClick.AddListener(OnCreateButtonClick);
        closeButton.onClick.AddListener(Close);
        weekChangeButton.onClick.AddListener(OnChangeWeekButtonClick);

        taskType.onValueChanged.AddListener(OnTypeToggleChanged);
        inAllWeekToggle.onValueChanged.AddListener(OnAllWeekToggleChanged);
        CalendarButton.onClick.AddListener(OnCalendarButtonClick);
        calendar.Callback = OnDaySelected;

        //Days = dayGo.GetComponentsInChildren<DayInfoButton>().ToList();

        weekParentGo.SetActive(false);
        daysContainerRect.gameObject.SetActive(false);

        Init();
    }

    public void Init()
    {
        _weekManager.Weeks.ForEach(w => 
        {
            //DayInfoButton controller = Instantiate(dayWeekItemGo, weekParentGo.transform);

            var dayInfo = _uiManager.CreatePanel<DayInfo>(weekParentGo.GetComponent<RectTransform>());

            var controller = dayInfo.gameObject.AddComponent<DayInfoButton>();

            controller.Init(w, OnWeekButtonClick);

            //controller.Week = w;
            //controller.text.text = w.WeekName;
            //controller.button.onClick.AddListener(() => OnWeekButtonClick(controller));
        });

        Week = _weekManager.Weeks?.FirstOrDefault();

        if (Week != null)
        {
            selectedWeekText.text = Week.WeekName;
        }

        Day = DayOfWeek.Monday;

        //Days.ForEach(d => d.button.onClick.AddListener(() => OnDayButtonClick(d)));

        foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
        {
            var day = _uiManager.CreatePanel<DayInfo>(daysContainerRect);

            var controller = day.gameObject.AddComponent<DayInfoButton>();

            controller.Init(dayOfWeek, OnDayButtonClick);
        }
    }

    private void OnChangeWeekButtonClick()
    {
        weekParentGo.SetActive(true);
    }

    //TODO: обработать исключения
    private void OnCreateButtonClick()
    {
        if(nameInputField.text == "")
        {
            nameInputField.placeholder.color = Color.red;

            return;
        }

        if (Week == null)
        {
            _weekManager.Create(TextStorage.UserFirstWeek);

            Week = _weekManager.Weeks.FirstOrDefault();
        }

        if (inAllWeekToggle.isOn)
        {
            _weekManager.Weeks.ForEach(w => CreateTask(w));
        }
        else
        {
            CreateTask(Week);
        }

        Close();
    }

    private void CreateTask(WeekController week)
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

        _taskManager.Create(week, newTask);
    }

    private void OnWeekButtonClick(DayInfoButton item)
    {
        Week = item.Week;

        selectedWeekText.text = Week.WeekName;

        weekParentGo.gameObject.SetActive(false);
    }

    private void OnDayButtonClick(DayInfoButton item)
    {
        Day = item.Info.dayOfWeek;

        //dayChangeButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(DayOfWeek), Day);

        selectedDayText.text = item.Info.Capture;

        daysContainerRect.gameObject.SetActive(false);
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
        _signalBus.Fire(new EnableBackgroundSignal(false));

        base.Close();
    }
}