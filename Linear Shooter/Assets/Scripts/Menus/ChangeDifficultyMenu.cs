using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Utilities.MenuSystem;
using Utilities.GameData;

public class ChangeDifficultyMenu : MonoBehaviour
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
    Text _titleComponent;
    [SerializeField]
    Text _descriptionComponent;

    [SerializeField]
    Button _rightButton;
    [SerializeField]
    Button _leftButton;
    [SerializeField]
    Button _selectButton;

    SliderKeyObserver _sliderKeyObserver;

    DifficultyInfo[] _difficultiesInfo;
    int _currentIndex = 0;
    int _maxIndexInList = 0;

    /* 
    .#####...#####....####...#####...######..#####...######..######..######...####..
    .##..##..##..##..##..##..##..##..##......##..##....##......##....##......##.....
    .#####...#####...##..##..#####...####....#####.....##......##....####.....####..
    .##......##..##..##..##..##......##......##..##....##......##....##..........##.
    .##......##..##...####...##......######..##..##....##....######..######...####..
    ................................................................................
    */
    DifficultyInfo _CurrentDifficultyInfo => _difficultiesInfo[_currentIndex];
    Difficulties _CurrenDifficulty
    {
        get
        {
            Enum.TryParse(_CurrentDifficultyInfo.Name, out Difficulties result);
            return result;
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
        MenuManager.Initialize();
        GameDataUtils.Initialize();

        _rightButton.onClick.AddListener(HandleRightButtonClick);
        _leftButton.onClick.AddListener(HandleLeftButtonClick);
        _selectButton.onClick.AddListener(HandleSelectButtonClick);

        _sliderKeyObserver = gameObject.AddComponent<SliderKeyObserver>();
        _sliderKeyObserver.LeftButton = _leftButton;
        _sliderKeyObserver.RightButton = _rightButton;
    }

    void Start()
    {
        _difficultiesInfo = GameDataUtils.DifficultiesInfo;
        _maxIndexInList = _difficultiesInfo.Length - 1;
        RefreshUI();
    }
    #endregion

    #region Event Handling
    public void HandleLeftButtonClick()
    {
        if (_currentIndex <= 0)
            return;
        _currentIndex--;
        RefreshUI();
    }
    public void HandleRightButtonClick()
    {
        if (_currentIndex >= _maxIndexInList)
            return;
        _currentIndex++;
        RefreshUI();
    }
    public void HandleSelectButtonClick()
    {
        PlayerPrefs.SetInt("SelectedDifficulty", (int)_CurrenDifficulty);
        PlayerPrefs.Save();
        MenuManager.GoToMenu(Menus.MainMenu);
    }
    #endregion

    void RefreshUI()
    {
        _titleComponent.text = _CurrentDifficultyInfo.Name;
        _descriptionComponent.text = _CurrentDifficultyInfo.Description;
    }
}
