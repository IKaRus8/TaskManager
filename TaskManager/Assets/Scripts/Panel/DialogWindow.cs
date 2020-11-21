using Assets.Scripts.DI.Signals;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class DialogWindow : BaseTempElement
{
    [SerializeField]
    protected Button okButton;
    [SerializeField]
    protected Button cancelButton;

    [SerializeField]
    [Range(0.05f, 0.3f)]
    private float speedScale;

    [SerializeField]
    [Range(0f, 300f)]
    private float yOffset;

    public UnityAction Action { get; set; }

    private float height;
    private float anchoredX;
    private float speed;

    private BackRaycaster raycaster;

    private UIManager _uiManager;

    [Inject]
    private void Construct(UIManager uIManager)
    {
        _uiManager = uIManager;
    }

    protected override void Awake()
    {
        base.Awake();

        var parentTransform = rectTransform.parent.GetComponent<RectTransform>();

        raycaster = _uiManager.CreatePanel<BackRaycaster>(parentTransform);
        raycaster.Action = Close;

        okButton.onClick.AddListener(() => Action?.Invoke());

        okButton.onClick.AddListener(Close);
        cancelButton.onClick.AddListener(Close);

        height = rectTransform.rect.height;
        anchoredX = rectTransform.anchoredPosition.x;
        speed = height * speedScale;

        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        while (rectTransform.anchoredPosition.y < yOffset)
        {
            float newPos = rectTransform.anchoredPosition.y + speed;

            rectTransform.anchoredPosition = new Vector2(anchoredX, Mathf.Min(yOffset, newPos));

            yield return new WaitForFixedUpdate();
        }

        SetParent(raycaster.GetComponent<RectTransform>());
    }

    private IEnumerator HideCoroutine()
    {
        while (rectTransform.anchoredPosition.y > - height)
        {
            float newPos = rectTransform.anchoredPosition.y - speed;

            rectTransform.anchoredPosition = new Vector2(anchoredX, Mathf.Max(- height, newPos));

            yield return new WaitForFixedUpdate();
        }

        raycaster.Close();
        base.Close();
    }

    public override void Close()
    {
        StartCoroutine(HideCoroutine());
    }
}