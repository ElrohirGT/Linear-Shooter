using UnityEngine;
using Utilities.MenuSystem;

public class HelpMenu : MonoBehaviour
{
    private void Awake() => MenuManager.Initialize();

    public void HandleBackToMainMenuButtonClick()
    {
        MenuManager.GoToMenu(Menus.MainMenu);
    }
}
