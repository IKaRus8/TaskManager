using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BackRaycaster : BaseTempElement, IPointerClickHandler
{
    public UnityAction Action { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        Action?.Invoke();

        //Close();
    }
}
