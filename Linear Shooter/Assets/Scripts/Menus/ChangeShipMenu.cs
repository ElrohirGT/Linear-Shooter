using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Configuration;
using Utilities.Constants;
using Utilities.GameData;
using Utilities.MenuSystem;

public class ChangeShipMenu : MonoBehaviour
{
    /*
    .######..######..######..##......#####....####..
    .##........##....##......##......##..##..##.....
    .####......##....####....##......##..##...####..
    .##........##....##......##......##..##......##.
    .##......######..######..######..#####....####..
    ................................................
    */
    [SerializeField]
    Text _shipDescriptionComponent;
    [SerializeField]
    Button _rightButton;
    [SerializeField]
    Button _leftButton;
    [SerializeField]
    Button _playButton;
    [SerializeField]
    Button _goBackButton;

    [SerializeField]
    Image _shipDisplayComponent;
    //Must be in the same order than the GameData.json file
    [SerializeField]
    List<Sprite> _shipsUnlockedDisplaySprites;
    [SerializeField]
    List<Sprite> _shipsLockedDisplaySprites;

    string[] _shipsInfoDictionaryKeys;
    int _currentShipIndex = 0;
    int _maxListIndex;

    SliderKeyObserver _sliderKeyObserver;

    /* 
    .#####...#####....####...#####...######..#####...######..######..######...####..
    .##..##..##..##..##..##..##..##..##......##..##....##......##....##......##.....
    .#####...#####...##..##..#####...####....#####.....##......##....####.....####..
    .##......##..##..##..##..##......##......##..##....##......##....##..........##.
    .##......##..##...####...##......######..##..##....##....######..######...####..
    ................................................................................
    */
    ShipInfo _CurrentShip => GameDataUtils.ShipsInfo[_shipsInfoDictionaryKeys[_currentShipIndex]];
    bool _IsCurrentShipUnlocked => _CurrentShip.Unlocked;
    Sprite _CurrentSprite
    {
        get
        {
            if (_CurrentShip.Unlocked)
                return _shipsUnlockedDisplaySprites[_currentShipIndex];
            return _shipsLockedDisplaySprites[_currentShipIndex];
        }
    }

    /*
    .##...##..######..######..##..##...####...#####....####..
    .###.###..##........##....##..##..##..##..##..##..##.....
    .##.#.##..####......##....######..##..##..##..##...####..
    .##...##..##........##....##..##..##..##..##..##......##.
    .##...##..######....##....##..##...####...#####....####..
    .........................................................
    */
    #region Unity
    void Awake()
    {
        GameDataUtils.Initialize();
        ConfigurationUtils.Initialize();
        MenuManager.Initialize();

        _leftButton.onClick.AddListener(HandleLeftButtonClick);
        _rightButton.onClick.AddListener(HandleRightButtonClick);
        _playButton.onClick.AddListener(HandlePlayButtonClick);
        _goBackButton.onClick.AddListener(HandleBackToMainMenuButtonClick);

        _sliderKeyObserver = gameObject.AddComponent<SliderKeyObserver>();
        _sliderKeyObserver.LeftButton = _leftButton;
        _sliderKeyObserver.RightButton = _rightButton;
    }
    void Start()
    {

        _shipsInfoDictionaryKeys = new string[GameDataUtils.ShipsInfo.Count];
        GameDataUtils.ShipsInfo.Keys.CopyTo(_shipsInfoDictionaryKeys, 0);

        _maxListIndex = _shipsInfoDictionaryKeys.Length - 1;
        RefreshUI();
    }
    #endregion

    #region Menu Event Handling
    public void HandleLeftButtonClick()
    {
        if (_currentShipIndex <= 0)
            return;
        _currentShipIndex--;
        RefreshUI();
    }
    public void HandleRightButtonClick()
    {
        if (_currentShipIndex >= _maxListIndex)
            return;
        _currentShipIndex++;
        RefreshUI();
    }
    public void HandlePlayButtonClick()
    {
        if (_IsCurrentShipUnlocked)
        {
            PlayerPrefs.SetString(PlayerPrefsConstants.SELECTED_SHIP, _shipsInfoDictionaryKeys[_currentShipIndex]);
            MenuManager.ExitMenuAndLoadScene("MainGame");
            return;
        }
        //Play an error sound
    }
    public void HandleBackToMainMenuButtonClick()
    {
        MenuManager.GoToMenu(Menus.MainMenu);
    }
    #endregion

    #region Refreshing the UI
    /// <summary>
    /// Main method that refreshes the UI.
    /// </summary>
    void RefreshUI()
    {
        _shipDisplayComponent.sprite = _CurrentSprite;
        _shipDisplayComponent.sprite.texture.filterMode = FilterMode.Point;
        _shipDescriptionComponent.text = GenerateCurrentShipInfoDisplay();
    }

    /// <summary>
    /// Creates the information display of the currently active ship.
    /// </summary>
    /// <returns>The formatted display of the current ship in display.</returns>
    string GenerateCurrentShipInfoDisplay()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Name: {_CurrentShip.DisplayName}");
        if (_CurrentShip.Unlocked)
        {
            sb.AppendLine($"Initial Lives: {_CurrentShip.InitialLives}");
            sb.AppendLine($"Ultimate Description: {_CurrentShip.UltimateDescription}");
        }
        else
        {
            sb.AppendLine($"Initial Lives: ---");
            sb.AppendLine($"Ultimate Description: {new string('-', 8)}");
            sb.AppendLine($"To Unlock: {_CurrentShip.ToUnlockMessage}");
        }

        return sb.ToString();
    }
    #endregion
}
