using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class PanelFactory<TPanel> : PlaceholderFactory<TPanel> where TPanel : ITempElement
    {
        public TPanel Create(RectTransform param)
        {
            TPanel elem = base.Create();

            elem.SetParent(param);

            return elem;
        }
    }
}
