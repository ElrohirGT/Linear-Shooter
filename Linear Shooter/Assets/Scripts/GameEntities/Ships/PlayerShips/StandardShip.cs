using System;
using System.Collections;
using System.Collections.Generic;
using Events;
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
        const string IDLE_ANIMATION_NAME = "StandardShip_IDLE";
        const string ULTIMATE_ANIMATION_PREFIX = "StandardShip_Ultimate";

        Timer _ultimateTimer;
        float _ultimateDuration;

        int _medalsCollectedSinceUltimateStarted = 0;

        PlayerLaserBulletGun _shipGun;

        protected override (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues()
        {
            return (
                ConfigurationUtils.StandardShipConfig.InitialHitpoints,
                ConfigurationUtils.StandardShipConfig.InitialHitpoints,
                ConfigurationUtils.StandardShipConfig.BaseDamage,
                ConfigurationUtils.StandardShipConfig.DamageCooldownDuration
            );
        }

        protected override int GetMinMendalsToUltimate() => ConfigurationUtils.StandardShipConfig.MinMedalsToUltimate;

        protected override IShipGun<Bullet> CreateShipGun() => _shipGun = gameObject.AddComponent<PlayerLaserBulletGun>();

        protected override ShipMotorSettings CreateMotorSettings() => new ShipMotorSettings(
                ConfigurationUtils.StandardShipConfig.RotationSpeed,
                ConfigurationUtils.StandardShipConfig.ThrustAmount
            );

        protected override ShipGunSettings CreateShipGunSettings()
        {
            Dictionary<string, GunConfig> gunsConfig = ConfigurationUtils.StandardShipConfig.Guns;

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
            _ultimateDuration = ConfigurationUtils.StandardShipConfig.UltimateDuration;

            ShipCollectedMedal += HandleShipCollectedMedal;
        }

        void HandleShipCollectedMedal(PlayerPickedUpMedalEventInfo obj)
        {
            if (_ultimateTimer.IsRunning)
            {
                _medalsCollectedSinceUltimateStarted++;
                return;
            }

            if (obj.PickedUpMedalsCount <= MinMedalsToUltimate)
                Animator.Play($"{ULTIMATE_ANIMATION_PREFIX}{obj.PickedUpMedalsCount}");
        }

        /// <summary>
        /// Shoots the ultimate of this ship.
        /// </summary>
        protected override void ShootUltimate()
        {
            StartCoroutine(ReturnToIDLESpriteAfterUltimateFinished());

            _shipGun.SetDamageMultiplier(2);
            shipGunSettings.ShootCooldownDuration /= 2;

            _ultimateTimer.StartTimer(_ultimateDuration);
        }

        IEnumerator ReturnToIDLESpriteAfterUltimateFinished()
        {
            int spriteNumber = MinMedalsToUltimate;
            float secondsTillNextSpriteChange = _ultimateDuration / MinMedalsToUltimate;
            while (_ultimateTimer.IsRunning)
            {
                yield return new WaitForSeconds(secondsTillNextSpriteChange);
                Animator.Play($"{ULTIMATE_ANIMATION_PREFIX}{spriteNumber}");
            }

            if (_medalsCollectedSinceUltimateStarted == 0)
                Animator.Play(IDLE_ANIMATION_NAME);
            else
                Animator.Play($"{ULTIMATE_ANIMATION_PREFIX}{_medalsCollectedSinceUltimateStarted}");

            _medalsCollectedSinceUltimateStarted = 0;
        }

        //Reset entity to previous state.
        void HandleUltimateTimerFinished()
        {
            _shipGun.SetDamageMultiplier(1);
            shipGunSettings.ShootCooldownDuration = ConfigurationUtils.StandardShipConfig.Guns[_shipGun.BulletTypeName].ShootCooldownDuration;

            OnUltimateEnded();
        }
    }
}