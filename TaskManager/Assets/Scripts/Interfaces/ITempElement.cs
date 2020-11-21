using UnityEngine;

public interface ITempElement
{
    /// <summary>
    /// Задать родительский трансформ
    /// </summary>
    /// <param name="parent">Родительский трансформ</param>
    void SetParent(RectTransform transform);

    /// <summary>
    /// Закрыть елемент
    /// </summary>
    void Close();
}