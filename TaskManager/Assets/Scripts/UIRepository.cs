using Assets.Scripts.Panel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UIRepository", menuName = "ScriptableObjects/UIRepository")]
public class UIRepository : ScriptableObject
{
    public List<BaseTempElement> prefabs;

	public TElement GetElement<TElement>() where TElement : BaseTempElement
	{
		BaseTempElement element = prefabs.FirstOrDefault(e => typeof(TElement).Equals(e.GetType()));

		return (TElement)element;
	}
}