using UnityEngine;

/// <summary>
/// Цветовая схема
/// </summary>
[CreateAssetMenu(fileName = "ColorRepository", menuName = "ScriptableObjects/ColorRepository")]
public class ColorRepository : ScriptableObject
{
    [SerializeField]
    public ColorTag[] colors;
}
