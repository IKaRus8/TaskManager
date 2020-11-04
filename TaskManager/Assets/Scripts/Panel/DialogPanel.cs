using Assets.Scripts.Panel;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class DialogPanel : BaseTempElement
{
    public InputField input;
    public Button okButton;
    public Button cancelButton;

    public UnityAction<string> Action { get; set; }

    [Inject]
    PanelManager panelManager;

    protected override void Awake()
    {
        base.Awake();

        input.onEndEdit.AddListener(OnInputFieldChange);

        okButton.onClick.AddListener(() => Action.Invoke(input.text));
        okButton.onClick.AddListener(Close);
        cancelButton.onClick.AddListener(Close);
    }

    private void OnInputFieldChange(string text)
    {
        if (text != "")
        {
            okButton.interactable = true;
        }
        else
        {
            okButton.interactable = false;
        }
    }

    public override void Close()
    {
        panelManager.EnableBackground(false);

        base.Close();
    }
}