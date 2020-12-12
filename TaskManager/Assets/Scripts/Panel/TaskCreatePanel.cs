using Assets.Scripts.DI.Signals;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TaskCreatePanel : BaseTempElement
{
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

    [SerializeField]
    private RectTransform mainTransform;
    
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Text sliderDescription;

    [Space]
    public DayInfoButton dayWeekItemGo;

    [Space]
    public InputField nameInputField;
    public InputField descriptionInputField;
    public RectTransform daysContainerRect;
    public GameObject weekParentGo;

    private SignalBus _signalBus;
    private WeekManager _weekManager;
    private UIManager _uiManager;

    private WeekController Week { get; set; }
    private DayOfWeek Day { get; set; }
    private DateTime TaskDate { get; set; }

    [Inject]
    private void Construct(SignalBus signalBus, WeekManager weekManager, UIManager uiManager)
    {
        _signalBus = signalBus;
        _weekManager = weekManager;
        _uiManager = uiManager;
    }

    protected override void Awake()
    {
        base.Awake();

        createButton.onClick.AddListener(OnCreateButtonClick);
        closeButton.onClick.AddListener(Close);
        weekChangeButton.onClick.AddListener(OnChangeWeekButtonClick);

        CalendarButton.onClick.AddListener(OnCalendarButtonClick);

        slider.onValueChanged.AddListener(OnSliderShange); 

        sliderDescription.text = TextStorage.TaskByWeek;

        weekParentGo.SetActive(false);
        daysContainerRect.gameObject.SetActive(false);

        Init();
    }

    public void Init()
    {
        _weekManager.Weeks.ForEach(w => 
        {
            var dayInfo = _uiManager.CreatePanel<DayInfo>(weekParentGo.GetComponent<RectTransform>());

            var controller = dayInfo.gameObject.AddComponent<DayInfoButton>();

            controller.Init(w, OnWeekButtonClick);
        });

        Week = _weekManager.Weeks?.FirstOrDefault();

        if (Week != null)
        {
            selectedWeekText.text = Week.WeekName;
        }

        Day = DayOfWeek.Monday;

        foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
        {
            var day = _uiManager.CreatePanel<DayInfo>(daysContainerRect);

            var controller = day.gameObject.AddComponent<DayInfoButton>();

            controller.Init(dayOfWeek, OnDayButtonClick);
        }
    }

    private void OnSliderShange(float value)
    {
        switch (value)
        {
            case 0:
                sliderDescription.text = TextStorage.OnceActiveTask;
                break;
            case 1:
                sliderDescription.text = TextStorage.TaskByWeek;
                break;
            case 2:
                sliderDescription.text = TextStorage.AllWeekTask;
                break;
        }

        weekChangeButton.gameObject.SetActive(value == 1);
        dayChangeButton.gameObject.SetActive(value != 0);
        CalendarButton.gameObject.SetActive(value == 0);
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

        if (slider.value == 2)
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
            isRecurring = slider.value == 1,
            _name = nameInputField.text,
            _descriptionText = descriptionInputField.text,
            _weekName = week.WeekName,
            _dayOfWeek = Day,
            _date = TaskDate
        };

        _signalBus.Fire(new TaskCreateSignal(week, newTask));
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

        selectedDayText.text = item.Info.Capture;

        daysContainerRect.gameObject.SetActive(false);
    }

    private void OnCalendarButtonClick()
    {
        var calendar = _uiManager.CreatePanel<CalendarPanel>(mainTransform);

        calendar.Callback = OnDaySelected;

        calendar.Show();
    }

    private void OnDaySelected(DateTime date)
    {
        var dateText = CalendarButton.GetComponentInChildren<Text>();

        if(dateText != null)
        {
            dateText.text = date.ToShortDateString();
        }

        TaskDate = date;
    }

    public override void Close()
    {
        _signalBus.Fire(new EnableBackgroundSignal(false));

        base.Close();
    }
}