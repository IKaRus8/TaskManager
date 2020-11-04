using Assets.Scripts;
using DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class PanelManager : MonoBehaviour
{
    public RectTransform panelBack;

    public RectTransform taskConatainer;

    //TODO: создавать из префабов
    public List<BasePanel> panels;

    [Inject]
    private DiContainer diContainer;

    public void EnableBackground(bool value)
    {
        panelBack.gameObject.SetActive(value);
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

    public T CreatePanel<T>(RectTransform parent) where T : ITempElement
    {
        var factory = diContainer.Resolve<PanelFactory<T>>();

        return factory.Create(parent);
    }

    public void WeekDialogConstruct(int weekCount, UnityAction<string> callback)
    {
        var panel = CreatePanel<DialogPanel>(panelBack);

        if (panel != default)
        {
            panel.input.text = TextStorage.Week + " " + weekCount + 1;
            panel.Action = callback;

            SwitchOffPanels();
            EnableBackground(true);
        }
    }
}
