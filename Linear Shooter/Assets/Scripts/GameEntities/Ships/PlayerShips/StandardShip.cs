using System.Collections.Generic;
using GameEntities.Bullets;
using GameEntities.Ships.Guns;
using GameEntities.Ships.Motors;
using UnityEngine;
using Utilities;
using Utilities.Configuration;

namespace GameEntities.Ships.PlayerShips
{
    public class StandardShip : PlayerShip
    {
        SpriteRenderer _spriteRenderer;

        Timer _ultimateTimer;
        float _ultimateDuration;

        PlayerLaserBulletGun _shipGun;

        protected override (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues()
        {
            return (
                ConfigurationUtils.PlayerShipsConfig.StandardShip.InitialHitpoints,
                ConfigurationUtils.PlayerShipsConfig.StandardShip.InitialHitpoints,
                ConfigurationUtils.PlayerShipsConfig.StandardShip.BaseDamage,
                ConfigurationUtils.PlayerShipsConfig.StandardShip.DamageCooldownDuration
            );
        }

        protected override int GetMinMendalsToUltimate() => ConfigurationUtils.PlayerShipsConfig.StandardShip.MinMedalsToUltimate;

        protected override IShipGun<Bullet> CreateShipGun() => _shipGun = gameObject.AddComponent<PlayerLaserBulletGun>();

        protected override ShipMotorSettings CreateMotorSettings() => new ShipMotorSettings(
                ConfigurationUtils.PlayerShipsConfig.StandardShip.RotationSpeed,
                ConfigurationUtils.PlayerShipsConfig.StandardShip.ThrustAmount
            );

        protected override ShipGunSettings CreateShipGunSettings()
        {
            Dictionary<string, GunConfig> gunsConfig = ConfigurationUtils.PlayerShipsConfig.StandardShip.Guns;

            foreach (var gunConfig in gunsConfig)
                if (gunConfig.Key.Equals(_shipGun.BulletTypeName))
                    return new ShipGunSettings(
                            gunConfig.Value.ShootCooldownDuration,
                            gunConfig.Value.BulletInitialImpulseMagnitude
                        );

            return null;
        }

        void Start()
        {
            _ultimateTimer = gameObject.AddComponent<Timer>();
            _ultimateTimer.Finished += HandleUltimateTimerFinished;
            _ultimateDuration = ConfigurationUtils.PlayerShipsConfig.StandardShip.UltimateDuration;

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Shoots the ultimate of this ship.
        /// </summary>
        protected override void ShootUltimate()
        {
            _spriteRenderer.color = new Color32(255, 215, 0, 255);

            _shipGun.SetDamageMultiplier(2);
            shipGunSettings.ShootCooldownDuration /= 2;

            _ultimateTimer.StartTimer(_ultimateDuration);
        }

        //Reset entity to previous state.
        private void HandleUltimateTimerFinished()
        {
            _shipGun.SetDamageMultiplier(1);
            shipGunSettings.ShootCooldownDuration = ConfigurationUtils.PlayerShipsConfig.StandardShip.Guns[_shipGun.BulletTypeName].ShootCooldownDuration;

            _spriteRenderer.color = Color.white;
            OnUltimateEnded();
        }
    }
}