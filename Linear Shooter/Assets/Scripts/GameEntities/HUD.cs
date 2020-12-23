using UnityEngine;
using UnityEngine.UI;
using Events;
using GameEntities.Ships.Enemies;
using Utilities.Constants;
using Utilities;
using System.Collections;
using System;
using GameEntities.PowerUps;

/// <summary>
/// Controls the UI displays in the scene.
/// </summary>
public class HUD : MonoBehaviour
{
    /*
    .######..######..######..##......#####....####..
    .##........##....##......##......##..##..##.....
    .####......##....####....##......##..##...####..
    .##........##....##......##......##..##......##.
    .##......######..######..######..#####....####..
    ................................................
    */

    #region Messages
    [SerializeField]
    Text _messagesDisplay;
    Timer _messagesDisplayTimer;
    readonly float _messagesDisplayTime = 2;
    Color _messagesDisplayColor;
    Coroutine _messageDisplayCoroutine;
    #endregion

    #region Lives
    [SerializeField]
    /// <summary>
    /// Get's the text component from the UI that displays lives.
    /// </summary>
    private Text _livesDisplay;

    int _currentLives = 0;

    /// <summary>
    /// Get's the prefix to use when updating the lives display.
    /// </summary>
    private readonly string _livesDisplayPrefix = "Lives: ";
    #endregion

    #region Score

    /// <summary>
    /// Get's the current score just for display.
    /// </summary>
    float _currentScore = 0;

    [SerializeField]
    /// <summary>
    /// Get's the text component of the UI that displays the score.
    /// </summary>
    private Text _scoreDisplay;

    /// <summary>
    /// Get's the prefix to use when updating the score display.
    /// </summary>
    private readonly string _scoreDisplayPrefix = "";
    #endregion

    #region Highscore
    /// <summary>
    /// Get's the current highscore just for display.
    /// </summary>
    float _currentHighScore = 0;
    [SerializeField]
    /// <summary>
    /// Get's the text component of the UI that displays the highscore.
    /// </summary>
    private Text _highscoreDisplay;
    /// <summary>
    /// Get's the prefix to use when updating the highscore display.
    /// </summary>
    private readonly string _highscoreDisplayPrefix = "Highscore: ";
    #endregion

    #region Medal Counter
    int _currentMedalCount;
    [SerializeField]
    Text _medalCountDisplay;
    int _maxMedalCount;
    #endregion

    /*
    .##...##..######..######..##..##...####...#####....####..
    .###.###..##........##....##..##..##..##..##..##..##.....
    .##.#.##..####......##....######..##..##..##..##...####..
    .##...##..##........##....##..##..##..##..##..##......##.
    .##...##..######....##....##..##...####...#####....####..
    .........................................................
    */
    void Start()
    {
        _messagesDisplayTimer = gameObject.AddComponent<Timer>();

        Player.PlayerHitpointsChanged += HandlePlayerHitpointsChanged;
        Player.PlayerCanShootUltimate += HandlePlayerCanShootUltimate;
        Player.PlayerPickedUpMedal += HandlePlayerPickedUpMedal;
        Player.PlayerIsShootingUltimate += HandlePlayerIsShootingUltimate;

        ShipEnemy.EnemyDied += HandleEnemyDied;

        FreezerPowerUp.PickedUp += HandleFreezerPowerUpPickedUp;

        _currentHighScore = PlayerPrefs.GetFloat(PlayerPrefsConstants.PLAYER_HIGHSCORE);
        _currentLives = Player.RemainingLives;
        _maxMedalCount = Player.MedalsForUltimate;

        UpdateDisplay();
    }

    private void HandleFreezerPowerUpPickedUp(FreezerPowerUpPickedUpEventInfo obj) => ShowMessage("FREEZE!", Color.cyan);

    private void HandlePlayerIsShootingUltimate()
    {
        _currentMedalCount = 0;
        UpdateDisplay();
    }

    private void HandlePlayerPickedUpMedal(PlayerPickedUpMedalEventInfo obj)
    {
        _currentMedalCount = Mathf.Clamp(obj.PickedUpMedalsCount, 0, _maxMedalCount);
        UpdateDisplay();
    }

    void OnDestroy()
    {
        Player.PlayerHitpointsChanged -= HandlePlayerHitpointsChanged;
        Player.PlayerCanShootUltimate -= HandlePlayerCanShootUltimate;
        Player.PlayerPickedUpMedal -= HandlePlayerPickedUpMedal;
        Player.PlayerIsShootingUltimate -= HandlePlayerIsShootingUltimate;

        ShipEnemy.EnemyDied -= HandleEnemyDied;

        FreezerPowerUp.PickedUp -= HandleFreezerPowerUpPickedUp;
    }

    private void HandlePlayerCanShootUltimate() => ShowMessage("ULTIMATE LOADED!", Color.yellow);

    /// <summary>
    /// Updates the lives display.
    /// </summary>
    private void HandlePlayerHitpointsChanged(PlayerHitpointsChangedEventInfo e)
    {
        _currentLives = e.RemainingLives;
        UpdateDisplay();
    }
    /// <summary>
    /// Updates the score display and if necessary the highscore display.
    /// </summary>
    private void HandleEnemyDied(EnemyDiedEventInfo e)
    {
        _currentScore += e.PointsWorth;

        if (_currentScore > _currentHighScore)
            _currentHighScore = _currentScore;

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        _highscoreDisplay.text = $"{_highscoreDisplayPrefix}{_currentHighScore:N0}";
        _scoreDisplay.text = $"{_scoreDisplayPrefix}{_currentScore:N0}";
        _livesDisplay.text = $"{_livesDisplayPrefix}{_currentLives}";
        _medalCountDisplay.text = $"{_currentMedalCount}/{_maxMedalCount}";
    }

    void ShowMessage(string message, Color color)
    {
        _messagesDisplayColor = color;
        _messagesDisplay.color = color;
        _messagesDisplay.text = message;

        _messagesDisplayTimer.StartTimer(_messagesDisplayTime);
        if (_messageDisplayCoroutine != null)
            StopCoroutine(_messageDisplayCoroutine);
        _messageDisplayCoroutine = StartCoroutine(FadeMessage());
    }
    IEnumerator FadeMessage()
    {
        while (_messagesDisplayTimer.IsRunning)
        {
            _messagesDisplayColor.a = _messagesDisplayTimer.RemainingSeconds / _messagesDisplayTime;
            _messagesDisplay.color = _messagesDisplayColor;
            yield return null;
        }
    }
}