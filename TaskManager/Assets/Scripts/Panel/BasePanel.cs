using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
