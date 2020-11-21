using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ����������� UI ��������
/// </summary>
[CreateAssetMenu(fileName = "UIRepository", menuName = "ScriptableObjects/UIRepository")]
public class UIRepository : ScriptableObject
{
    public List<BaseTempElement> prefabs;

	/// <summary>
	/// �������� ������ �� ����
	/// </summary>
	/// <typeparam name="TElement">��� �������</typeparam>
	public TElement GetPrefab<TElement>() where TElement : BaseTempElement
	{
		BaseTempElement element = prefabs.FirstOrDefault(e => typeof(TElement).Equals(e.GetType()));

		return (TElement)element;
	}
}