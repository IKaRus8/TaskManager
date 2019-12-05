using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public InputField input;
    public Button okButton;
    public Button cancelButton;

    public UnityAction<string> Action { get; set; }

    private void Awake()
    {
        input.onEndEdit.AddListener(OnInputFieldChange);

        okButton.onClick.AddListener(() => Action.Invoke(input.text));
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
