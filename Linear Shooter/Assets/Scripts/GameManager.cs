using Events;
using UnityEngine;
using Utilities;
using Utilities.MenuSystem;
using Utilities.Configuration;
using Utilities.AudioSystem;
using Utilities.GameData;
using Utilities.Constants;
using System;
using Spawners.WaveSpawners;

/// <summary>
/// Controls the state of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    /*
    .######..######..######..##......#####....####..
    .##........##....##......##......##..##..##.....
    .####......##....####....##......##..##...####..
    .##........##....##......##......##..##......##.
    .##......######..######..######..#####....####..
    ................................................
    */
    /// <summary>
    /// Get's the current player of the game.
    /// </summary>
    Player _player;

    /// <summary>
    /// Get's the gameobject where all the spawners are in the game.
    /// </summary>
    [SerializeField]
    GameObject _spawnersContainer;

    /// <summary>
    /// Get's all the wave spawners in the scene.
    /// </summary>
    WaveSpawner[] _waveSpawners;

    /// <summary>
    /// Get's how many spawners finished their respective wave (all enemies from that wave are dead).
    /// </summary>
    int _spawnersThatFinished;

    /// <summary>
    /// Get's the difficulty the player is currently playing. By default is Easy.
    /// </summary>
    public static Difficulties SelectedDifficulty { get; private set; }

    /// <summary>
    /// An event that fires once all spawners have to change wave.
    /// </summary>
    public static Action AllSpawnersFinished;

    /*
    .##...##..######..######..##..##...####...#####....####..
    .###.###..##........##....##..##..##..##..##..##..##.....
    .##.#.##..####......##....######..##..##..##..##...####..
    .##...##..##........##....##..##..##..##..##..##......##.
    .##...##..######....##....##..##...####...#####....####..
    .........................................................
    */
    private void Awake()
    {
        Initialize();
        SetUpWorldBorders();
        ScreenUtils.ScreenSizeChanged += HandleScreenSizeChanged;

        if (PlayerPrefs.HasKey(PlayerPrefsConstants.SELECTED_DIFFICULTY))
            SelectedDifficulty = (Difficulties)PlayerPrefs.GetInt(PlayerPrefsConstants.SELECTED_DIFFICULTY);

        _player = gameObject.AddComponent<Player>();
        Player.PlayerDied += HandlePlayerDies;

        //Listen to spawners events.
        _waveSpawners = _spawnersContainer.GetComponentsInChildren<WaveSpawner>();
        foreach (var waveSpawner in _waveSpawners)
            waveSpawner.WaveFinished += HandleWaveEntitiesDestroyed;
    }

    private void HandleWaveEntitiesDestroyed()
    {
        _spawnersThatFinished++;
        if (_spawnersThatFinished >= _waveSpawners.Length)
            OnNextWave();
    }

    private void OnNextWave()
    {
        _spawnersThatFinished = 0;
        AllSpawnersFinished?.Invoke();
    }

    /// <summary>
    /// Is called on the awake method, initializes all utilities and spawners,
    /// and other stuff that need to be done before the game starts.
    /// </summary>
    private void Initialize()
    {
        ScreenUtils.ForceInitialize();
        ConfigurationUtils.ForceInitialize();
        GameDataUtils.ForceInitialize();
        MenuManager.ForceInitialize();
        AudioManager.ForceInitialize(gameObject.AddComponent<AudioSource>());
    }

    /// <summary>
    /// changes the world borders to acomodate the new size of the screen.
    /// </summary>
    private void HandleScreenSizeChanged() => SetUpWorldBorders();

    /// <summary>
    /// Set-ups the world borders, this function should be called again if the ScreenSize changes.
    /// </summary>
    private void SetUpWorldBorders()
    {
        BoxCollider2D worldBorder = gameObject.AddComponent<BoxCollider2D>();
        worldBorder.size = new Vector2(ScreenUtils.WorldRight * 2, ScreenUtils.WorldTop * 2);
        worldBorder.isTrigger = true;
    }

    /// <summary>
    /// Once the player dies the game should end, this handles that.
    /// </summary>
    private void HandlePlayerDies(PlayerDiedEventInfo e)
    {
        float highscore = float.MinValue;

        PlayerPrefs.SetFloat(PlayerPrefsConstants.PLAYER_LAST_SCORE, e.Score);

        if (PlayerPrefs.HasKey(PlayerPrefsConstants.PLAYER_HIGHSCORE))
            highscore = PlayerPrefs.GetFloat(PlayerPrefsConstants.PLAYER_HIGHSCORE);

        if (highscore < e.Score)
        {
            PlayerPrefs.SetFloat(PlayerPrefsConstants.PLAYER_HIGHSCORE, e.Score);
            PlayerPrefs.Save();
        }
        MenuManager.GoToMenu(Menus.GameOverMenu);
    }

}