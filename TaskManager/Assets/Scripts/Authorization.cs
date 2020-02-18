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
                    message.text = "unvalidate password";
                }
            }
            else
            {
                message.text = "unvalidate login";
            }
        }

        private void SignIn()
        {
            bool isLogin = MongoDbAtlasManager.TakeUser(login.text, password.text);

            if (isLogin)
            {
                _panelManager.SwitchOffPanels();
                _panelManager.EnableBackground(false);
            }
            else
            {
                message.text = "User not finded";
            }
        }

        private void SignUp()
        {
            bool isLogin = MongoDbAtlasManager.NewUser(login.text, password.text);

            if (isLogin)
            {
                _panelManager.SwitchOffPanels();
                _panelManager.EnableBackground(false);
            }
            else
            {
                message.text = "User already created"; 
            }
        }
    }
}