using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DataBase
{
    class Authorization : MonoBehaviour
    {
        public InputField login;
        public InputField password;
        public Text message;
        public Button authorizeButton;
        public Toggle registrationToggle;

        public MongoDbAtlasManager dbManager;

        private Regex regex;
        private string regexPattern = "^[a-zA-Z0-9]{3,8}$";

        private void Awake()
        {
            authorizeButton.onClick.AddListener(OnButtonClick);

            regex = new Regex(regexPattern);
        }

        private void Start()
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