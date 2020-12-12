using Assets.Scripts;
using Assets.Scripts.DI.Signals;
using DataBase;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Менеджер UI
/// </summary>
public class UIManager : MonoBehaviour
{
    public RectTransform panelBack;
    public RectTransform taskConatainer;
    [SerializeField]
    private Button openMenuButton;
    [SerializeField]
    private MenuController menuController;

    //TODO: создавать из префабов
    [SerializeField]
    private List<BasePanel> panels;

    private DiContainer _diContainer;
    private SignalBus _signalBus;

    [Inject(Id = "mainCanvas")]
    private Canvas _mainCanvas;
    private RectTransform canvasRectTransform;

    //TODO: сдеалть счетчик тех кому нужен фон
    private int neededBackgroundCount;

    [Inject]
    private void Construct(DiContainer diContainer, SignalBus signalBus)
    {
        _diContainer = diContainer;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        canvasRectTransform = _mainCanvas.GetComponent<RectTransform>();

        openMenuButton.onClick.AddListener(OnOpenMenuButtonClick);

        _signalBus.Subscribe<EnableBackgroundSignal>(EnableBackground);
        _signalBus.Subscribe<SwitchOffPanelsSignal>(SwitchOffPanels);

        CreatePanel<AuthorizationPanel>(panelBack);
    }

    private void OnOpenMenuButtonClick()
    {
        menuController.Show();
    }

    private void EnableBackground(EnableBackgroundSignal signal)
    {
        panelBack.gameObject.SetActive(signal.Value);
    }

    private void SwitchOffPanels(SwitchOffPanelsSignal signal)
    {
        panels.Where(p => p != signal.Panel && p.gameObject.activeSelf).ToList().ForEach(p => p.Close());
    }

    /// <summary>
    /// Получить панель по типу
    /// </summary>
    /// <typeparam name="T">Тип панели</typeparam>
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

    /// <summary>
    /// Создать панель заданного типа
    /// </summary>
    /// <typeparam name="T">Тип панели</typeparam>
    /// <param name="parent">Родительский трансформ</param>
    public T CreatePanel<T>(RectTransform parent) where T : ITempElement
    {
        var factory = _diContainer.Resolve<PanelFactory<T>>();

        return factory.Create(parent ?? canvasRectTransform );
    }
}