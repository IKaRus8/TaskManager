using UnityEngine;
using UnityEngine.UI;

public class HomeScreenController : MonoBehaviour
{
    [SerializeField]
    private Button openMenuButton;

    [SerializeField]
    private MenuController menuController;

    private void Awake()
    {
        openMenuButton.onClick.AddListener(OnOpenMenuButtonClick);
    }

    private void OnOpenMenuButtonClick()
    {
        menuController.Show();
    }
}
