using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace DataBase
{
    public class Authorization : BasePanel
    {
        public InputField login;
        public InputField password;
        public Text message;
        public Button authorizeButton;
        public Toggle registrationToggle;

        private PanelManager _panelManager => PanelManager.Instance;
        private TaskManager _taskManager => TaskManager.Instance;

        private Regex regex;
        private string regexPattern = "^[a-zA-Z0-9]{3,10}$";

        protected override void Awake()
        {
            authorizeButton.onClick.AddListener(OnButtonClick);

            regex = new Regex(regexPattern);
        }

        protected override void Start()
        {
            MongoDbAtlasManager.Conect();

            ForceLogin();
        }

        private void ForceLogin()
        {
            User user = MongoDbAtlasManager.TakeUser("NewUser", "test");

            if (user != null)
            {
                _panelManager.SwitchOffPanels();
                _panelManager.EnableBackground(false);

                _taskManager.OnAuthorization(user.weeks);

                MessageManager.SetFooterInfo($"Привет {user.login}");
            }
        }

        private void OnButtonClick()
        {
            if (login.text != "" && regex.IsMatch(login.text))
            {
                if (password.text != "" && regex.IsMatch(password.text))
                {
                    if (registrationToggle.isOn)
                    {
                        SignUp();
                    }
                    else
                    {
                        SignIn();
                    }
                }
                else
                {
                    message.text = StaticTextStorage.UnvalidatePassword;
                }
            }
            else
            {
                message.text = StaticTextStorage.UnvalidateLogin;
            }
        }

        private void SignIn()
        {
            User user = MongoDbAtlasManager.TakeUser(login.text, password.text);

            if (user != null)
            {
                _panelManager.SwitchOffPanels();
                _panelManager.EnableBackground(false);

                _taskManager.OnAuthorization(user.weeks);

                MessageManager.SetFooterInfo(StaticTextStorage.Hello + " " + user.login);
            }
            else
            {
                message.text = StaticTextStorage.User404;
            }
        }

        private void SignUp()
        {
            User user = MongoDbAtlasManager.NewUser(login.text, password.text);

            if (user != null)
            {
                _panelManager.SwitchOffPanels();
                _panelManager.EnableBackground(false);

                _taskManager.OnAuthorization(user.weeks);

                MessageManager.SetFooterInfo(StaticTextStorage.Hello + " " + user.login);
            }
            else
            {
                message.text = StaticTextStorage.UserAlreadyCreated;
            }
        }
    }
}