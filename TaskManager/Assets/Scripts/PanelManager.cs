using DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    public GameObject panelBack;

    public GameObject taskConatainer;

    //TODO: создавать из префабов
    public List<BasePanel> panels;
    public List<TempPanel> prefabs;

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
        //костыль
        panels.Where(p => p.gameObject.activeSelf).ToList().ForEach(p => p.Close());
    }

    public void SwitchOffPanels(BasePanel panel)
    {
        //TODO: оптимизировать
        panels.Where(p => p != panel && p.gameObject.activeSelf).ToList().ForEach(p => p.Close());
    }

    public T GetPanel<T>() where T : BasePanel
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

    public T CreatePanel<T>(Transform parent) where T : TempPanel
    {
        var prefab = prefabs.FirstOrDefault(p => p is T);

        if (prefab != null)
        {
            var panel = Instantiate(prefab, parent);

            return panel.GetComponent<T>();
        }
        else
        {
            return default;
        }
    }

    public void FillTask(List<TaskInfo> tasks)
    {

    }
}
