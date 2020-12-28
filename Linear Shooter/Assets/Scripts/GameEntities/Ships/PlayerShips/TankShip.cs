using System;
using System.Collections;
using System.Collections.Generic;
using GameEntities.Bullets;
using GameEntities.Ships.Guns;
using GameEntities.Ships.Motors;
using UnityEngine;
using Utilities;
using Utilities.Configuration;

namespace GameEntities.Ships.PlayerShips
{
    public class TankShip : PlayerShip
    {
        const string IDLE_ANIMATION_NAME = "TankShip_IDLE";
        const string ACTIVATE_ULTIMATE_ANIMATION_NAME = "TankShip_ActivateUltimate";
        const string DEACTIVATE_ULTIMATE_ANIMATION_NAME = "TankShip_DeactivateUltimate";
        const string ULTIMATE_ANIMATION_NAME = "TankShip_Ultimate";

        Timer _ultimateTimer;
        float _ultimateDuration;

        float _rotationScaleFactor;
        float _thrustScaleFactor;

        PlayerHeavyLaserBulletGun _shipGun;

        protected override ShipMotorSettings CreateMotorSettings() => new ShipMotorSettings(
                ConfigurationUtils.TankShipConfig.RotationSpeed,
                ConfigurationUtils.TankShipConfig.ThrustAmount
            );

        protected override IShipGun<Bullet> CreateShipGun() => _shipGun = gameObject.AddComponent<PlayerHeavyLaserBulletGun>();

        protected override ShipGunSettings CreateShipGunSettings()
        {
            Dictionary<string, GunConfig> gunsConfig = ConfigurationUtils.TankShipConfig.Guns;

            foreach (var gunConfig in gunsConfig)
                if (gunConfig.Key.Equals(_shipGun.BulletTypeName))
                    return new ShipGunSettings(
                        gunConfig.Value.ShootCooldownDuration,
                        gunConfig.Value.BulletInitialImpulseMagnitude
                    );

            return null;
        }

        protected override (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues() => (
                ConfigurationUtils.TankShipConfig.MaxHitpoints,
                ConfigurationUtils.TankShipConfig.MaxHitpoints,
                ConfigurationUtils.TankShipConfig.BaseDamage,
                ConfigurationUtils.TankShipConfig.DamageCooldownDuration
            );

        protected override int GetMinMendalsToUltimate() => ConfigurationUtils.TankShipConfig.MinMedalsToUltimate;

        private void Start()
        {
            _ultimateTimer = gameObject.AddComponent<Timer>();
            _ultimateTimer.Finished += HandleUltimateTimerFinished;
            _ultimateDuration = ConfigurationUtils.TankShipConfig.UltimateDuration;

            _rotationScaleFactor = ConfigurationUtils.TankShipConfig.RotationScaleFactor;
            _thrustScaleFactor = ConfigurationUtils.TankShipConfig.ThrustScaleFactor;
        }

        protected override void ShootUltimate()
        {
            MakeInvincible();

            ShipMotorSettings.ScaleSettings(_rotationScaleFactor, _thrustScaleFactor);
            _ultimateTimer.StartTimer(_ultimateDuration);

            Animator.Play(ACTIVATE_ULTIMATE_ANIMATION_NAME);
            StartCoroutine(ActivateUltimateAnimation());
        }
        void HandleUltimateTimerFinished()
        {
            MakeVincible();

            ShipMotorSettings.ScaleSettings(1 / _rotationScaleFactor, 1 / _thrustScaleFactor);

            Animator.Play(DEACTIVATE_ULTIMATE_ANIMATION_NAME);
            StartCoroutine(DeactivateUltimateAnimation());

            OnUltimateEnded();
        }

        IEnumerator ActivateUltimateAnimation()
        {
            yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
            Animator.Play(ULTIMATE_ANIMATION_NAME);
        }
        IEnumerator DeactivateUltimateAnimation()
        {
            yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
            Animator.Play(IDLE_ANIMATION_NAME);
        }
    }
}