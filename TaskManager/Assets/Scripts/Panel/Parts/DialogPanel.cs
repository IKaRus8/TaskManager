﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogPanel : BasePanel, ITempPanel
{
    public InputField input;
    public Button okButton;
    public Button cancelButton;

    public UnityAction<string> Action { get; set; }

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
        PanelManager.Instance.EnableBackground(false);

        Destroy(gameObject);
    }
}