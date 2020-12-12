using Assets.Scripts;
using Assets.Scripts.DI.Signals;
using DataBase;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public UIManager panelManager;

    public UIRepository uIRepository;

    public Canvas mainCanvas;

    public Camera mainCamera;

    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromInstance(panelManager).AsSingle();

        Container.Bind<TaskService>().AsSingle();

        Container.Bind<WeekManager>().AsSingle();

        Container.Bind<NotificationManager>().AsSingle();

        Container.Bind<Canvas>().WithId("mainCanvas").FromInstance(mainCanvas).AsSingle();

        Container.Bind<Camera>().WithId("mainCamera").FromInstance(mainCamera).AsSingle();

        BindFactories();

        BindSignal();
    }

    private void BindFactories()
    {
        Container.BindFactory<TaskCreatePanel, PanelFactory<TaskCreatePanel>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<TaskCreatePanel>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<BaseTask, PanelFactory<BaseTask>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<BaseTask>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<DialogWindowInput, PanelFactory<DialogWindowInput>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<DialogWindowInput>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<DialogWindow, PanelFactory<DialogWindow>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<DialogWindow>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<WeekInfoPanel, PanelFactory<WeekInfoPanel>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<WeekInfoPanel>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<WeekListItem, PanelFactory<WeekListItem>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<WeekListItem>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<DayInfo, PanelFactory<DayInfo>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<DayInfo>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<AuthorizationPanel, PanelFactory<AuthorizationPanel>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<AuthorizationPanel>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<BackRaycaster, PanelFactory<BackRaycaster>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<BackRaycaster>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<CalendarPanel, PanelFactory<CalendarPanel>>()
                 .FromComponentInNewPrefab(uIRepository.GetPrefab<CalendarPanel>())
                 .UnderTransform(mainCanvas.transform);
    }

    private void BindSignal()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<EnableBackgroundSignal>();

        Container.DeclareSignal<SwitchOffPanelsSignal>();

        Container.DeclareSignal<SetActiveTutorialSignal>();

        Container.DeclareSignal<SendMessageSignal>();

        Container.DeclareSignal<TaskCreateSignal>();

        Container.DeclareSignal<TaskRemovedSignal>();

        Container.DeclareSignal<RemoveWeekSignal>();
    }
}