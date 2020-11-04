﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DataBase
{
    public class Authorization : BasePanel
    {
        [SerializeField]
        private InputField login;
        [SerializeField]
        private InputField password;
        [SerializeField]
        private Text message;
        [SerializeField]
        private Button authorizeButton;
        [SerializeField]
        private Button retryConect;
        [SerializeField]
        private Toggle registrationToggle;

        private PanelManager _panelManager;
        private TaskManager _taskManager;
        private WeekManager _weekManager;

        [Inject]
        private void Construct(PanelManager panelManager, TaskManager taskManager, WeekManager weekManager)
        {
            _panelManager = panelManager;
            _taskManager = taskManager;
            _weekManager = weekManager;
        }

        private Regex regex;
        private readonly string regexPattern = "^[a-zA-Z0-9]{3,10}$";

        private string noConect = "Не удалось установить подключение";

        protected override void Awake()
        {
            authorizeButton.onClick.AddListener(OnButtonClick);
            registrationToggle.onValueChanged.AddListener(OnRegestrationToggleChange);

            retryConect.onClick.AddListener(() => TryConect());

            regex = new Regex(regexPattern);
        }

        protected override void Start()
        {
            if (TryConect())
            {
                AutoLogin(); 
            }
        }

        private bool TryConect()
        {
            try
            {
                MongoDbAtlasManager.Conect();

                return true;
            }
            catch
            {
                message.text = noConect;

                retryConect.gameObject.SetActive(true);

                return false;
            }
        }

        private void AutoLogin()
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