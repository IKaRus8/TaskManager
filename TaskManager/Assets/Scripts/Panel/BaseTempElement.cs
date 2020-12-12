using UnityEngine;

/// <summary>
/// Базовый класс временых UI объектов
/// </summary>
public class BaseTempElement : BasePanel, ITempElement
{
    public void SetParent(RectTransform parent)
    {
        transform.SetParent(parent, false);
    }

    public override void Close()
    {
        Destroy(gameObject);
    }
}