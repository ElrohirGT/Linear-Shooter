using UnityEngine;
using UnityEngine.UI;
using Utilities.Constants;
using Utilities.MenuSystem;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    Button PlayAgainButton;

    [SerializeField]
    Button GoBackButton;

    [SerializeField]
    Text _scoreDisplay;

    private void Awake()
    {
        float score = PlayerPrefs.GetFloat(PlayerPrefsConstants.PLAYER_LAST_SCORE);
        float highscore = PlayerPrefs.GetFloat(PlayerPrefsConstants.PLAYER_HIGHSCORE);
        MenuManager.Initialize();
        _scoreDisplay.text = score.ToString("n0");

        if (score == highscore)
            _scoreDisplay.color = new Color(255, 215, 0);

        PlayAgainButton.onClick.AddListener(HandlePlayAgainButtonClickedEvent);
        GoBackButton.onClick.AddListener(HandleGoBackButtonClickedEvent);
    }

    private void HandleGoBackButtonClickedEvent() => MenuManager.GoToMenu(Menus.MainMenu);

    private void HandlePlayAgainButtonClickedEvent() => MenuManager.ExitMenuAndLoadScene("MainGame");
}
