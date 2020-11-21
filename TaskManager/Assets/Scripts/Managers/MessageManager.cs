using Assets.Scripts.DI.Signals;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MessageManager : MonoBehaviour
{
    public Text headerCaption;
    public Text footerInfo;
    public Text menuCaption;
    public GameObject tutorial;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        signalBus.Subscribe<SendMessageSignal>(SetMessageText);
        signalBus.Subscribe<SetActiveTutorialSignal>(ShowHideTutorial);
    }

    private void SetMessageText(SendMessageSignal signal)
    {
        switch (signal.Target)
        {
            case MessageTarget.Header :
                headerCaption.text = signal.Message;
                break;

            case MessageTarget.Footer:
                footerInfo.text = signal.Message;
                break;

            case MessageTarget.Menu:
                menuCaption.text = signal.Message;
                break;
        }
    }

    public void ShowHideTutorial(SetActiveTutorialSignal signal)
    {
        tutorial.SetActive(signal.Value);
    }
}
