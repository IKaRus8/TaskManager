using DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    public GameObject panelBack;

    //TODO: создавать из префабов
    public List<BasePanel> panels;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void EnableBackground(bool value)
    {
        panelBack.SetActive(value);
    }

    public void SwitchOffPanels()
    {
        panels.ForEach(p => p.Close());
    }

    public void SwitchOffPanels(BasePanel panel)
    {
        //TODO: оптимизировать
        panels.Where(p => p != panel).ToList().ForEach(p => p.Close());
    }

    public T GetPanel<T>()
    {
        var panel = panels.FirstOrDefault(p => p is T);

        if (panel != null)
        {
            return panel.GetComponent<T>();
        }
        else
        {
            return default;
        }
    }
}
