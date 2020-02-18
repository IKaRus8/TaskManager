using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTask : MonoBehaviour
{
    public Text description;
    public Image background;

    public TaskInfo taskInfo;

    protected Color defColor;

    virtual public void Awake()
    {
        taskInfo._isDone = false;
        defColor = background.color;

        description.text = taskInfo._descriptionText;
    }

    virtual public void Start()
    {
        
    }

    virtual public void Update()
    {
        
    }

    virtual public void Construct(string text)
    {
        taskInfo._descriptionText = text;
    }

    virtual public void Change(string text)
    {

    }

    virtual public void Remove()
    {
        taskInfo.deleted = true;
        Destroy(gameObject);
    }

    virtual protected void OnToggleValueChanged(bool value) 
    {
        taskInfo._isDone = value;
    }
}
