using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Репозиторий UI префабов
/// </summary>
[CreateAssetMenu(fileName = "UIRepository", menuName = "ScriptableObjects/UIRepository")]
public class UIRepository : ScriptableObject
{
    public List<BaseTempElement> prefabs;

	/// <summary>
	/// Получить префаб по типу
	/// </summary>
	/// <typeparam name="TElement">Тип префаба</typeparam>
	public TElement GetPrefab<TElement>() where TElement : BaseTempElement
	{
		BaseTempElement element = prefabs.FirstOrDefault(e => typeof(TElement).Equals(e.GetType()));

		return (TElement)element;
	}
}