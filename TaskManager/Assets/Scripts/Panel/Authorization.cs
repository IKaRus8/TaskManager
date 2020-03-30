﻿using System.Collections.Generic;
using System.Linq;
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
        private WeekManager _weekManager => WeekManager.Instance;

        private Regex regex;
        private readonly string regexPattern = "^[a-zA-Z0-9]{3,10}$";

        protected override void Awake()
        {
            authorizeButton.onClick.AddListener(OnButtonClick);
            registrationToggle.onValueChanged.AddListener(OnRegestrationToggleChange);

            regex = new Regex(regexPattern);
        }

        protected override void Start()
        {
            MongoDbAtlasManager.Conect();

            ForceLogin();
        }

        private void ForceLogin()
        {
            var login = PlayerPrefs.GetString(TextStorage.Login);
            var password = PlayerPrefs.GetString(TextStorage.Password);

            if(!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                SignIn(login, password);
            }
        }

        private void OnButtonClick()
        {
            if (login.text != "" && regex.IsMatch(login.text))
            {
                if (password.text != "" && regex.IsMatch(password.text))
                {
                    bool isLogin;

                    if (registrationToggle.isOn)
                    {
                        isLogin = SignUp();
                    }
                    else
                    {
                        isLogin = SignIn(login.text, password.text);
                    }

                    if (isLogin)
                    {
                        PlayerPrefs.SetString(TextStorage.Login, login.text);
                        PlayerPrefs.SetString(TextStorage.Password, password.text);
                    }
                }
                else
                {
                    message.text = TextStorage.UnvalidatePassword;
                }
            }
            else
            {
                message.text = TextStorage.UnvalidateLogin;
            }
        }

        private bool SignIn(string login, string password)
        {
            User user = MongoDbAtlasManager.TakeUser(login, password);

            if (user != null)
            {
                Login(user);

                return true;
            }
            else
            {
                message.text = TextStorage.User404;

                return false;
            }
        }

        private bool SignUp()
        {
            User user = MongoDbAtlasManager.NewUser(login.text, password.text);

            if (user != null)
            {
                Login(user);

                return true;
            }
            else
            {
                message.text = TextStorage.UserAlreadyCreated;

                return false;
            }
        }

        private void Login(User user)
        {
            _panelManager.SwitchOffPanels();
            _panelManager.EnableBackground(false);

            OnAuthorization(user.weeks);

            MessageManager.SetFooterInfo(TextStorage.Hello + ", " + user.login);
        }

        private void OnRegestrationToggleChange(bool value)
        {
            var text = authorizeButton.GetComponentInChildren<Text>();

            if (text != null)
            {
                if (value)
                {
                    text.text = TextStorage.SignUp;
                }
                else
                {
                    text.text = TextStorage.SignIn;
                }
            }
        }

        private void OnAuthorization(List<WeekController> weeks)
        {
            if (weeks != null && weeks.Any())
            {
                _weekManager.Add(weeks);

                _taskManager.LoadTasks();

                _taskManager.ShowTodayTasks();
            }
            //после регистрации
            else if (weeks != null && !weeks.Any())
            {
                _weekManager.Add(TextStorage.UserFirstWeek);
            }
        }
    }
}