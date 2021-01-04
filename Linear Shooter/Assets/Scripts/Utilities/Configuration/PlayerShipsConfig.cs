using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Utilities.Constants;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;

namespace Utilities.Configuration
{
    public partial class PlayerShipsConfig
    {
        [JsonConstructor]
        public PlayerShipsConfig(StandardShipConfig standardShip, GrenadierShipConfig grenadierShip, TankShipConfig tankShip)
        {
            StandardShip = standardShip;
            GrenadierShip = grenadierShip;
            TankShip = tankShip;
        }

        [J("StandardShip", Required = R.Always)] public StandardShipConfig StandardShip { get; }
        [J("GrenadierShip", Required = R.Always)] public PlayerShipConfig GrenadierShip { get; }
        [J("TankShip", Required = R.Always)] public TankShipConfig TankShip { get; }

        public Dictionary<string, PlayerShipConfig> ToDictionary() => new Dictionary<string, PlayerShipConfig>()
            {
                { "StandardShip", StandardShip },
                { "GrenadierShip", GrenadierShip },
                { "TankShip", TankShip }
            };
    }

    public class GrenadierShipConfig : PlayerShipConfig
    {
        [JsonConstructor]
        public GrenadierShipConfig(float minScoreToUnlock, int minMedalsToUltimate, float rotationSpeed, float thrustAmount, Dictionary<string, GunConfig> guns, float maxHitpoints, float initialHitpoints, float baseDamage, float damageCooldownDuration) : base(minScoreToUnlock, minMedalsToUltimate, rotationSpeed, thrustAmount, guns, maxHitpoints, initialHitpoints, baseDamage, damageCooldownDuration)
        {
        }

        public override bool Condition()
        {
            //TODO delete this override, this is just for the moment the grenadier ship is no implemented.
            return false;
        }
    }

    public abstract class PlayerShipConfig : ShipConfig
    {
        [JsonConstructor]
        public PlayerShipConfig(float minScoreToUnlock, int minMedalsToUltimate, float rotationSpeed, float thrustAmount, Dictionary<string, GunConfig> guns, float maxHitpoints, float initialHitpoints, float baseDamage, float damageCooldownDuration)
            : base(rotationSpeed, thrustAmount, guns, maxHitpoints, initialHitpoints, baseDamage, damageCooldownDuration)
        {
            MinMedalsToUltimate = minMedalsToUltimate;
            MinScoreToUnlock = minScoreToUnlock;
        }

        [J("MinMedalsToUltimate", Required = R.Always)] public int MinMedalsToUltimate { get; }
        [J("MinScoreToUnlock", Required = R.Always)] public float MinScoreToUnlock { get; }

        public virtual bool Condition()
        {
            return PlayerPrefs.GetFloat(PlayerPrefsConstants.PLAYER_HIGHSCORE) >= MinScoreToUnlock;
        }
    }

    public class TankShipConfig : PlayerShipConfig
    {
        [JsonConstructor]
        public TankShipConfig(float thrustScaleFactor, float rotationScaleFactor, float ultimateDuration, float minScoreToUnlock, int minMedalsToUltimate, float rotationSpeed, float thrustAmount, Dictionary<string, GunConfig> guns, float maxHitpoints, float initialHitpoints, float baseDamage, float damageCooldownDuration)
            : base(minScoreToUnlock, minMedalsToUltimate, rotationSpeed, thrustAmount, guns, maxHitpoints, initialHitpoints, baseDamage, damageCooldownDuration)
        {
            UltimateDuration = ultimateDuration;
            RotationScaleFactor = rotationScaleFactor;
            ThrustScaleFactor = thrustScaleFactor;
        }
        [J("RotationScaleFactor", Required = R.Always)] public float RotationScaleFactor { get; }
        [J("ThrustScaleFactor", Required = R.Always)] public float ThrustScaleFactor { get; }
        [J("UltimateDuration", Required = R.Always)] public float UltimateDuration { get; }
    }

    public class StandardShipConfig : PlayerShipConfig
    {
        [JsonConstructor]
        public StandardShipConfig(float ultimateDuration, float minScoreToUnlock, int minMedalsToUltimate, float rotationSpeed, float thrustAmount, Dictionary<string, GunConfig> guns, float maxHitpoints, float initialHitpoints, float baseDamage, float damageCooldownDuration)
            : base(minScoreToUnlock, minMedalsToUltimate, rotationSpeed, thrustAmount, guns, maxHitpoints, initialHitpoints, baseDamage, damageCooldownDuration)
        {
            UltimateDuration = ultimateDuration;
        }

        [J("UltimateDuration")] public float UltimateDuration { get; }
    }
}