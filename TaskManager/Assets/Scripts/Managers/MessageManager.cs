using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public Text headerCaption;
    public Text FooterInfo;
    public Text menuCaption;
    public GameObject tutorial;

    private static Text _headerCaption;
    private static Text _FooterInfo;
    private static Text _menuCaption;
    private static GameObject _tutorial;

    public static MessageManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _headerCaption = headerCaption;
        _FooterInfo = FooterInfo;
        _menuCaption = menuCaption;
        _tutorial = tutorial;
    }

    public static void SetHeaderCaption(string text)
    {
        _headerCaption.text = text;
    }

    public static void SetFooterInfo(string text)
    {
        _FooterInfo.text = text;
    }

    public static void SetMenuCaption(string text)
    {
        _menuCaption.text = text;
    }

    public static void ShowHideTutorial(bool value)
    {
        if (_tutorial.activeSelf != value)
        {
            _tutorial.SetActive(value); 
        }
    }
}
