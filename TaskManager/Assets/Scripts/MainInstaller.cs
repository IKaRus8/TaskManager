using Assets.Scripts;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public PanelManager panelManager;

    public UIRepository uIRepository;

    public Canvas mainCanvas;

    public Camera mainCamera;

    public override void InstallBindings()
    {
        BindFactories();

        Container.Bind<PanelManager>().FromInstance(panelManager).AsSingle();

        Container.Bind<TaskManager>().AsSingle();

        Container.Bind<WeekManager>().AsSingle();

        Container.Bind<NotificationManager>().AsSingle();

        Container.Bind<Canvas>().WithId("mainCanvas").AsSingle();

        Container.Bind<Camera>().WithId("mainCamera").AsSingle();
    }

    private void BindFactories()
    {
        Container.BindFactory<TaskCreatePanel, PanelFactory<TaskCreatePanel>>()
                 .FromComponentInNewPrefab(uIRepository.GetElement<TaskCreatePanel>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<BaseTask, PanelFactory<BaseTask>>()
                 .FromComponentInNewPrefab(uIRepository.GetElement<BaseTask>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<DialogPanel, PanelFactory<DialogPanel>>()
                 .FromComponentInNewPrefab(uIRepository.GetElement<DialogPanel>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<WeekInfoPanel, PanelFactory<WeekInfoPanel>>()
                 .FromComponentInNewPrefab(uIRepository.GetElement<WeekInfoPanel>())
                 .UnderTransform(mainCanvas.transform);

        Container.BindFactory<WeekListItem, PanelFactory<WeekListItem>>()
                 .FromComponentInNewPrefab(uIRepository.GetElement<WeekListItem>())
                 .UnderTransform(mainCanvas.transform);
    }
}
