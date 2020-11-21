using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseTask : BaseTempElement
{
    public Text taskName;
    public Image background;
    public Toggle doneToggle;
    public Color doneColor;

    [Space]
    public GameObject content;
    public Text description;
    public InputField inputField;
    public Button leftButton;
    public Button rightButton;
    public Toggle HeaderToggle;

    public TaskInfo taskInfo;

    protected Text leftButtonText;
    protected Text rightButtonText;

    protected UnityAction leftBtnAction;
    protected UnityAction rightBtnAction;

    protected Color defColor;
    private Color removeColor = Color.red;
    private Color changeColor = Color.yellow;
    private string oldtext;
    private bool changeMod;
    private bool awaked;

    protected override void Awake()
    {
        if (awaked)
        {
            return;
        }

        awaked = true;

        defColor = background.color;

        inputField.onValueChanged.AddListener(OnTaskDescriptionChange);
        HeaderToggle.onValueChanged.AddListener(ShowFullTaskView);

        leftButtonText = leftButton.GetComponentInChildren<Text>();
        rightButtonText = rightButton.GetComponentInChildren<Text>();

        leftButton.onClick.AddListener(OnLeftBtnClick);
        rightButton.onClick.AddListener(OnRightBtnClick);

        ToDefaulState();
    }

    public void Init(TaskInfo task)
    {
        if (!awaked)
        {
            Awake();
        }

        taskInfo = task;
        SetText();
        doneToggle.isOn = taskInfo._isDone;
        SetTaskView();

        doneToggle.onValueChanged.AddListener(OnDoneToggleValueChanged);
    }

    virtual protected void OnDoneToggleValueChanged(bool value)
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
        taskName.text = taskInfo._name;
        description.text = taskInfo._descriptionText;
    }

    private void ToDefaulState()
    {
        leftBtnAction = OnChangeButtonClick;
        leftButtonText.text = TextStorage.Change;

        rightBtnAction = OnRemoveButtonClick;
        rightButtonText.text = TextStorage.Remove;

        if (changeMod)
        {
            changeMod = false;
            description.text = oldtext;
        }

        if (inputField.gameObject.activeSelf)
        {
            inputField.gameObject.SetActive(false);
        }
    }

    protected virtual void Change()
    {
        taskInfo._descriptionText = description.text;
        StorageManager.Update(taskInfo);
        changeMod = false;

        ToDefaulState();
        SetTaskView();
    }

    protected virtual void Remove()
    {
        taskInfo.deleted = true;
        StorageManager.Update(taskInfo);
        Close();
    }

    protected virtual void Cancel()
    {
        ToDefaulState();
        SetTaskView();
    }

    protected virtual void OnLeftBtnClick()
    {
        leftBtnAction?.Invoke();
    }

    protected virtual void OnRightBtnClick()
    {
        rightBtnAction?.Invoke();
    }

    protected virtual void OnChangeButtonClick()
    {
        leftButtonText.text = TextStorage.Accept;
        leftBtnAction = Change;

        rightButtonText.text = TextStorage.Cancel;
        rightBtnAction = Cancel;

        oldtext = description.text;

        inputField.gameObject.SetActive(true);
        inputField.text = description.text;
    }

    protected virtual void OnRemoveButtonClick()
    {
        leftButtonText.text = TextStorage.Remove;
        leftBtnAction = Remove;

        rightButtonText.text = TextStorage.Cancel;
        rightBtnAction = Cancel;

        background.color = removeColor;
    }

    protected virtual void ShowFullTaskView(bool value)
    {
        if (value)
        {
            content.SetActive(true);
        }
        else
        {
            ToDefaulState();
            SetTaskView();

            content.SetActive(false);
        }
    }

    private void OnTaskDescriptionChange(string newText)
    {
        changeMod = true;

        description.text = newText;

        background.color = changeColor;
    }
}