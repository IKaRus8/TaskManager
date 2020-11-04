using Assets.Scripts.Panel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WeekInfoPanel : BaseTempElement
{
    public GameObject daysContainer;
    public GameObject dialogGO;
    public Text capture;
    public Text label;
    public Button backButton;
    public Button removeButton;
    public Button yesDialogButton;
    public Button noDialogButton;
    public List<DayInfo> days;

    private WeekController _week;
    private string dialogLabel = "Уверены?";

    private PanelManager _panelManager;
    private WeekManager _weekManager;

    [Inject]
    private void Construct(PanelManager panelManager, WeekManager weekManager)
    {
        _panelManager = panelManager;
        _weekManager = weekManager;
    }

    protected override void Awake()
    {
        base.Awake();

        backButton.onClick.AddListener(OnBackClick);

        RemoveButtonCallback();
    }

    public void Init(WeekController week)
    {
        _week = week;
        capture.text = week.WeekName;

        List<TaskInfo> tasks = UserInfo.Tasks; 

        tasks = tasks?.Where(t => t._weekName == week.WeekName)?.ToList();

        if (tasks != null && tasks.Any())
        {
            days.ForEach(d => 
            {
                List<TaskInfo> tasksOfDay = tasks.Where(t => (int)t._dayOfWeek == days.IndexOf(d) + 1).ToList();

                d.Init(tasksOfDay);
            });
        }
    }

    private void RemoveButtonCallback()
    {
        removeButton.onClick.AddListener(() => 
        {
            label.text = dialogLabel;
            removeButton.interactable = false;

            dialogGO.SetActive(true);
        });

        noDialogButton.onClick.AddListener(() =>
        {
            label.text = _week.WeekName;
            removeButton.interactable = true;

            dialogGO.SetActive(false);
        });

        yesDialogButton.onClick.AddListener(() =>
        {
            StorageManager.Remove(_week);
            _weekManager.Remove(_week.WeekName);

            OnBackClick();
        });
    }

    private void OnBackClick()
    {
        Close();
    }

    public override void Close()
    {
        _panelManager.EnableBackground(false);

        base.Close();
    }
}