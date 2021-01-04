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
    public class TankShip : PlayerShip
    {
        const string IDLE_ANIMATION_NAME = "TankShip_IDLE";
        const string ULTIMATE_PROGRESS_ANIMATION_PREFIX = "TankShip_Ultimate";
        const string ULTIMATE_ANIMATION_NAME = "TankShip_Ultimate";
        const string ACTIVATE_ULTIMATE_ANIMATION_NAME = "TankShip_ActivateUltimate";
        const string DEACTIVATE_ULTIMATE_ANIMATION_NAME = "TankShip_DeactivateUltimate";

        Timer _ultimateTimer;
        float _ultimateDuration;

        float _rotationScaleFactor;
        float _thrustScaleFactor;

        PlayerHeavyLaserBulletGun _shipGun;
        private int _medalsCollectedSinceUltimateStarted;

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
                Animator.Play($"{ULTIMATE_PROGRESS_ANIMATION_PREFIX}{obj.PickedUpMedalsCount}");
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
            //Wait a frame so the animator can change state, so the animation.length returns the true length of the animation
            yield return null;

            AnimatorStateInfo animation = Animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(animation.length);

            Animator.Play(ULTIMATE_ANIMATION_NAME);
        }
        IEnumerator DeactivateUltimateAnimation()
        {
            //Wait a frame so the animator can change state, so the animation.length returns the true length of the animation
            yield return null;

            AnimatorStateInfo animation = Animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(animation.length);

            if (_medalsCollectedSinceUltimateStarted == 0)
                Animator.Play(IDLE_ANIMATION_NAME);
            else
                Animator.Play($"{ULTIMATE_PROGRESS_ANIMATION_PREFIX}{_medalsCollectedSinceUltimateStarted}");

            _medalsCollectedSinceUltimateStarted = 0;
        }
    }
}