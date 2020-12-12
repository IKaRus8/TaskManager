using UnityEngine;

/// <summary>
/// �������� �����
/// </summary>
[CreateAssetMenu(fileName = "ColorRepository", menuName = "ScriptableObjects/ColorRepository")]
public class ColorRepository : ScriptableObject
{
    [SerializeField]
    public ColorTag[] colors;
}
