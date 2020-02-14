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

        public MongoDbAtlasManager dbManager;

        private Regex regex;
        private string regexPattern = "^[a-zA-Z0-9]{3,8}$";

        protected override void Awake()
        {
            authorizeButton.onClick.AddListener(OnButtonClick);

            regex = new Regex(regexPattern);
        }

        protected override void Start()
        {
            dbManager = new MongoDbAtlasManager();
            dbManager.Conect();
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
            message.text = dbManager.TakeUser(login.text, password.text);
        }

        private void SignUp()
        {
            message.text = dbManager.NewUser(login.text, password.text);
        }
    }
}