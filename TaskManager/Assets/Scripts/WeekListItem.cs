using Assets.Scripts.Panel;
using UnityEngine.UI;
using Zenject;

public class WeekListItem : BaseTempElement
{
    public Button showInfoButton;
    public Button removeButton;
    public Text label;

    private WeekController _week;

    [Inject]
    private PanelManager _panelManager;

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

        if(panel != null)
        {
            _panelManager.EnableBackground(true);

            panel.Init(_week);
        }
    }

    private void OnRemoveButtonClick()
    {
        Close();
    }
}
