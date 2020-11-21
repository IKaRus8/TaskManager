using Assets.Scripts.DI.Signals;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button createWeekButton;
    [SerializeField]
    private Button createTaskButton;
    [SerializeField]
    private Toggle weekToggle;
    [SerializeField]
    private RectTransform weekListContainer;
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    [Range(0.05f, 0.3f)]
    private float speedScale;

    private RectTransform rectTransform;
    private int weekCount;
    private float width;
    private float anchoredY;
    private float speed;

    private UIManager _panelManager;
    private TaskService _taskManager;
    private WeekManager _weekManager;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(UIManager panelManager, TaskService taskManager, WeekManager weekManager, SignalBus signalBus)
    {
        _panelManager = panelManager;
        _taskManager = taskManager;
        _weekManager = weekManager;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        createWeekButton.onClick.AddListener(_weekManager.WeekDialogConstruct);
        closeButton.onClick.AddListener(Hide);
        createTaskButton.onClick.AddListener(ShowTaskCreatePanel);
        weekToggle.onValueChanged.AddListener(OnWeekToggleChange);

        rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width;
        anchoredY = rectTransform.anchoredPosition.y;
        speed = width * speedScale;
    }

    private void OnGUI()
    {
        if(_weekManager?.Weeks?.Count != weekCount)
        {
            ClearWeekList();
            FillWeekList();
        }
    }

    private void OnEnable()
    {
        FillWeekList();
    }

    private void OnDisable()
    {
        ClearWeekList();
    }

    public void Show()
    {
        StartCoroutine(ShowCoroutine());
    }

    public void Hide()
    {
        StartCoroutine(HideCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        while (rectTransform.anchoredPosition.x < 0)
        {
            float newPos = rectTransform.anchoredPosition.x + speed;

            rectTransform.anchoredPosition = new Vector2(Mathf.Min(0, newPos), anchoredY);

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator HideCoroutine()
    {
        while (rectTransform.anchoredPosition.x > - width)
        {
            float newPos = rectTransform.anchoredPosition.x - speed;

            rectTransform.anchoredPosition = new Vector2(Mathf.Max(-width, newPos), anchoredY);

            yield return new WaitForFixedUpdate();
        }
    }

    private void FillWeekList()
    {
        _weekManager.Weeks.ForEach(w =>
        {
            WeekListItem item = _panelManager.CreatePanel<WeekListItem>(weekListContainer);

            if (item != null)
            {
                item.Init(w);
            }

            weekCount++;
        });
    }

    private void ClearWeekList()
    {
        var weeks = weekListContainer.GetComponentsInChildren<WeekListItem>();

        if (weeks != null && weeks.Any())
        {
            foreach (var t in weeks)
            {
                Destroy(t.gameObject);
            }
        }

        weekCount = 0;
    }

    public void ShowTaskCreatePanel()
    {
        _signalBus.Fire(new SwitchOffPanelsSignal(null));
        _signalBus.Fire(new EnableBackgroundSignal(true));

        _taskManager._taskCreatePanel = _panelManager.CreatePanel<TaskCreatePanel>(_panelManager.panelBack);
        _taskManager._taskCreatePanel.Show();
    }

    private void OnWeekToggleChange(bool value)
    {
        weekListContainer.gameObject.SetActive(value);
    }
}