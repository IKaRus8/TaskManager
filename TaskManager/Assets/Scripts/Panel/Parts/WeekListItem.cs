using Assets.Scripts.DI.Signals;
using UnityEngine.UI;
using Zenject;

public class WeekListItem : BaseTempElement
{
    public Button showInfoButton;
    public Button removeButton;
    public Text label;

    private WeekController _week;

    private UIManager _panelManager;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(UIManager panelManager, SignalBus signalBus)
    {
        _panelManager = panelManager;
        _signalBus = signalBus;
    }

    protected override void Awake()
    {
        base.Awake();

        showInfoButton.onClick.AddListener(OnShowInfoButtonClick);
        removeButton.onClick.AddListener(OnRemoveButtonClick);
    }

    public void Init(WeekController week)
    {
        _week = week;

        label.text = week.WeekName;
    }

    private void OnShowInfoButtonClick()
    {
        var panel = _panelManager.CreatePanel<WeekInfoPanel>(_panelManager.panelBack);

        if (panel != null)
        {
            _signalBus.Fire(new EnableBackgroundSignal(true));

            panel.Init(_week);
        }
    }

    private void OnRemoveButtonClick()
    {
        Close();
    }
}