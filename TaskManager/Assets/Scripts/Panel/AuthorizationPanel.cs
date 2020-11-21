using Assets.Scripts.DI.Signals;
using Assets.Scripts.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DataBase
{
    public class AuthorizationPanel : BaseTempElement
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

        private TaskService _taskManager;
        private WeekManager _weekManager;
        private SignalBus _signalBus;

        private Regex regex;
        private readonly string regexPattern = "^[a-zA-Z0-9]{3,10}$";

        private bool IsConect
        {
            get
            {
                try
                {
                    MongoDbAtlasManager.Conect();

                    return true;
                }
                catch
                {
                    message.text = TextStorage.NoConectMessage;

                    retryConect.gameObject.SetActive(true);

                    return false;
                }
            }
        }

        [Inject]
        private void Construct(TaskService taskManager, WeekManager weekManager, SignalBus signalBus)
        {
            _taskManager = taskManager;
            _weekManager = weekManager;
            _signalBus = signalBus;
        }

        protected override void Awake()
        {
            authorizeButton.onClick.AddListener(OnAuthorizeButtonClick);
            registrationToggle.onValueChanged.AddListener(OnRegestrationToggleChange);

            retryConect.onClick.AddListener(AutoLogin);

            regex = new Regex(regexPattern);

            AutoLogin();
        }

        private void AutoLogin()
        {
            if (IsConect)
            {
                var login = PlayerPrefs.GetString(TextStorage.Login);
                var password = PlayerPrefs.GetString(TextStorage.Password);

                if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
                {
                    SignIn(login, password);
                }
            }
        }

        private void OnAuthorizeButtonClick()
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

        private void Login(User user)
        {
            OnAuthorizationCompleted(user.weeks);

            _signalBus.Fire(new SendMessageSignal(MessageTarget.Footer, $"{TextStorage.Hello}, {user.login}!"));

            _signalBus.Fire(new EnableBackgroundSignal(false));

            Close();
        }

        private void OnAuthorizationCompleted(List<WeekController> weeks)
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
                _weekManager.Create(TextStorage.UserFirstWeek);
            }
        }
    }
}