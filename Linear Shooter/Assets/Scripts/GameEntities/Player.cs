using System;
using UnityEngine;
using Events;
using GameEntities.Ships.Enemies;
using GameEntities.Ships.PlayerShips;
using Utilities.Constants;

/// <summary>
/// Represents a player in the game.
/// </summary>
public class Player : MonoBehaviour
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
    /// Get's the player's ship.
    /// </summary>
    static PlayerShip _ship;

    /// <summary>
    /// Get's the player current score.
    /// </summary>
    private float _score;

    /* 
    .#####...#####....####...#####...######..#####...######..######..######...####..
    .##..##..##..##..##..##..##..##..##......##..##....##......##....##......##.....
    .#####...#####...##..##..#####...####....#####.....##......##....####.....####..
    .##......##..##..##..##..##......##......##..##....##......##....##..........##.
    .##......##..##...####...##......######..##..##....##....######..######...####..
    ................................................................................
    */
    /// <summary>
    /// Returns the position of the player ship in the game.
    /// </summary>
    public static Vector3 Position
    {
        get
        {
            if (_ship != null)
                return _ship.transform.position;
            return default;
        }
    }

    public static int RemainingLives
    {
        get
        {
            if (_ship != null)
                return (int)_ship.RemainingHitpoints;
            return default;
        }
    }
    public static int MedalsForUltimate
    {
        get
        {
            if (_ship != null)
                return _ship.MinMedalsToUltimate;
            return default;
        }
    }

    /*
    .######..##..##..######..##..##..######...####..
    .##......##..##..##......###.##....##....##.....
    .####....##..##..####....##.###....##.....####..
    .##.......####...##......##..##....##........##.
    .######....##....######..##..##....##.....####..
    ................................................
    */
    /// <summary>
    /// Event that fires when the player's ship took damage.
    /// </summary>
    public static Action<PlayerHitpointsChangedEventInfo> PlayerHitpointsChanged;

    /// <summary>
    /// Event that fires when the player dies.
    /// </summary>
    public static Action<PlayerDiedEventInfo> PlayerDied;

    /// <summary>
    /// Event that fires once the player can shoot it's ultimate.
    /// </summary>
    public static Action PlayerCanShootUltimate;
    /// <summary>
    /// Event that fires once the player decides to shoot it's ultimate.
    /// </summary>
    public static Action PlayerIsShootingUltimate;
    /// <summary>
    /// Event that fires once the player ultimate has ended.
    /// </summary>
    public static Action PlayerUltimateEnded;

    public static Action<PlayerPickedUpMedalEventInfo> PlayerPickedUpMedal;

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
        //Creating ship according to the one the player selected.
        string pathToPrefab = $"Prefabs/{PlayerPrefs.GetString(PlayerPrefsConstants.SELECTED_SHIP, null) ?? "StandardShip"}";
        PlayerShip objectToInstantiate = Resources.Load<PlayerShip>(pathToPrefab);
        _ship = Instantiate(objectToInstantiate);

        //Registering for events
        _ship.ShipCanShootUltimate += HandleShipCanShootUltimate;
        _ship.ShipShootingUltimate += HandleShipShootingUltimate;
        _ship.ShipUltimateEnded += HandleShipUltimateEnded;
        _ship.ShipCollectedMedal += HandleShipCollectedMedal;

        _ship.EntityTookDamage += HandleShipTookDamage;
        _ship.EntityHealed += HandleShipHealed;
        _ship.EntityDied += HandleShipDied;
        ShipEnemy.EnemyDied += HandleEnemyDied;
    }

    void OnDestroy()
    {
        ShipEnemy.EnemyDied -= HandleEnemyDied;
        _ship.EntityTookDamage -= HandleShipTookDamage;
        _ship.EntityDied -= HandleShipDied;
        _ship.EntityHealed -= HandleShipHealed;

        _ship.ShipCanShootUltimate -= HandleShipCanShootUltimate;
        _ship.ShipShootingUltimate -= HandleShipShootingUltimate;
        _ship.ShipUltimateEnded -= HandleShipUltimateEnded;
        _ship.ShipCollectedMedal -= HandleShipCollectedMedal;
    }
    #endregion

    #region EventHandling
    void HandleShipCollectedMedal(PlayerPickedUpMedalEventInfo obj) => PlayerPickedUpMedal?.Invoke(obj);
    void HandleEnemyDied(EnemyDiedEventInfo e) => _score += e.PointsWorth;

    void HandleShipUltimateEnded() => PlayerUltimateEnded?.Invoke();
    void HandleShipShootingUltimate() => PlayerIsShootingUltimate?.Invoke();
    void HandleShipCanShootUltimate() => PlayerCanShootUltimate?.Invoke();

    void HandleShipTookDamage(EntityHitpointsChanged e) => PlayerHitpointsChanged?.Invoke(new PlayerHitpointsChangedEventInfo((int)e.RemainingHitpoints));
    void HandleShipHealed(EntityHitpointsChanged e) => PlayerHitpointsChanged?.Invoke(new PlayerHitpointsChangedEventInfo((int)e.RemainingHitpoints));
    void HandleShipDied()
    {
        PlayerDied?.Invoke(new PlayerDiedEventInfo(_score));
        Destroy(gameObject);
    }
    #endregion
}