using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTask : MonoBehaviour
{
    public Text description;
    public Image background;

    public bool IsDone { get; set; }
    public string DescriptionText { get; set; }

    protected Color defColor;
    protected TaskType type;

    virtual public void Awake()
    {
        IsDone = false;
        defColor = background.color;

        description.text = DescriptionText;
    }

    virtual public void Start()
    {
        
    }

    virtual public void Update()
    {
        
    }

    virtual public void Construct(string text)
    {
        DescriptionText = text;
    }

    virtual public void Change(string text)
    {

    }

    virtual public void Remove()
    {
        type = TaskType.Deleted;
        Destroy(gameObject);
    }

    virtual protected void OnToggleValueChanged(bool value) 
    { 
        IsDone = value;
    }
}
