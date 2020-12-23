using UnityEngine;
using Utilities.MenuSystem;

public class MainMenu : MonoBehaviour
{
    private void Awake() => MenuManager.Initialize();

    public void HandlePlaybuttonClicked() => MenuManager.GoToMenu(Menus.ChangeShipMenu);
    public void HandleChangeDifficultyButtonClicked() => MenuManager.GoToMenu(Menus.ChangeDifficultyMenu);
    public void HandleHelpButtonClicked() => MenuManager.GoToMenu(Menus.HelpMenu);
    public void HandleQuitButtonClicked() => Application.Quit();
}
