using UnityEngine;

namespace Assets.Scripts.Panel
{
    public class BaseTempElement : BasePanel, ITempElement
    {
        public void SetParent(RectTransform parent)
        {
            transform.SetParent(parent);
        }

        public override void Close()
        {
            Destroy(gameObject);
        }
    }
}
