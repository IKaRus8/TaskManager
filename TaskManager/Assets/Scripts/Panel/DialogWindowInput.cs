using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Диалоговое окно с полем ввода
/// </summary>
public class DialogWindowInput : DialogWindow
{
    public InputField input;

    public UnityAction<string> ActionString { get; set; }

    protected override void Awake()
    {
        input.onEndEdit.AddListener(OnInputFieldChange);

        okButton.onClick.AddListener(() => ActionString?.Invoke(input.text));

        base.Awake();
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
}