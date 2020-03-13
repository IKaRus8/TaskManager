using UnityEngine;
using UnityEngine.UI;

public class BaseTask : MonoBehaviour, ITempPanel
{
    public Text taskName;
    public Text description;
    public Image background;
    public Toggle doneToggle;
    public Color doneColor;

    public TaskInfo taskInfo;

    protected Color defColor;

    virtual public void Awake()
    {
        defColor = background.color;
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

    public void Construct(TaskInfo task)
    {
        taskInfo = task;
        SetText();
        doneToggle.isOn = taskInfo._isDone;
        SetTaskView();

        doneToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    virtual public void Change(string text)
    {

    }

    virtual public void Remove()
    {
        taskInfo.deleted = true;
        Close();
    }

    virtual protected void OnToggleValueChanged(bool value)
    {
        taskInfo._isDone = value;

        SetTaskView();

        StorageManager.Update(taskInfo);
    }

    private void SetTaskView()
    {
        if (taskInfo._isDone)
        {
            background.color = doneColor;
        }
        else
        {
            background.color = defColor;
        }
    }

    public void SetText()
    {
        description.text = taskInfo._descriptionText;
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
