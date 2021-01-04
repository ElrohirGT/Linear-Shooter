using System;
using GameEntities.Pools;
using GameEntities.Shields;
using GameEntities.Ships.Motors;
using GameEntities.Ships.Motors.States;
using UnityEngine;
using Utilities;
using Utilities.Configuration;

namespace GameEntities.Ships.Enemies.MediumEnemies
{
    public class ShieldEnemy : ShipEnemy
    {
        [SerializeField]
        Shield _shield;

        Timer _shieldCooldownTimer;
        float _shieldCooldown;

        IState _initialShipMotorState;

        public override void ReturnToPool() => ShieldEnemyPool.Instance.ReturnToPool(this);

        protected override void Awake()
        {
            base.Awake();

            _shieldCooldownTimer = gameObject.AddComponent<Timer>();
            _shieldCooldown = ConfigurationUtils.ShieldEnemy.ShieldCooldown;
            _shieldCooldownTimer.Finished += HandleShieldCooldownTimerFinished;

            _shield.EntityDied += HandleShieldBroke;
        }
        private void OnEnable() => _shieldCooldownTimer.StartTimer(_shieldCooldown);
        void Start()
        {
            //This needs to be in the start method because _shield hasn't initialized yet if we were to use it on awake,
            //So the bound.size property would return 0,0,0.
            Collider2D shieldCollider = _shield.GetComponent<Collider2D>();
            float shieldColliderRadius = shieldCollider.bounds.size.x * shieldCollider.transform.localScale.x / 2;
            var protectState = new FindAndProtectEnemiesState(transform, shieldColliderRadius, ShipMotorInput);

            ShipMotorStateMachine.AddTransition(_initialShipMotorState, protectState, () => _shield.gameObject.activeInHierarchy);
            ShipMotorStateMachine.AddTransition(protectState, _initialShipMotorState, () =>
            {
                if (!_shield.gameObject.activeInHierarchy)
                    Debug.Log("Returning to initial state...");
                return !_shield.gameObject.activeInHierarchy;
            });
            DeactivateShield();
        }

        private void HandleShieldCooldownTimerFinished() => ActivateShield();
        private void HandleShieldBroke() => DeactivateShield();

        void ActivateShield() => _shield.gameObject.SetActive(true);
        void DeactivateShield()
        {
            _shield.gameObject.SetActive(false);
            _shieldCooldownTimer.StartTimer(_shieldCooldown);
        }

        protected override ShipMotorSettings CreateMotorSettings() => new ShipMotorSettings(
                ConfigurationUtils.ShieldEnemy.RotationSpeed,
                ConfigurationUtils.ShieldEnemy.ThrustAmount
            );

        protected override StateMachine CreateShipMotorStateMachine()
        {
            _initialShipMotorState = gameObject.AddComponent<FleeFromPlayerState>().Initialize(ShipMotorInput, ConfigurationUtils.ShieldEnemy.RotationCooldown);
            return new StateMachine(_initialShipMotorState);
        }

        protected override (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues() => (
                ConfigurationUtils.ShieldEnemy.MaxHitpoints,
                ConfigurationUtils.ShieldEnemy.InitialHitpoints,
                ConfigurationUtils.ShieldEnemy.BaseDamage,
                ConfigurationUtils.ShieldEnemy.DamageCooldownDuration
            );

        protected override float GetPointsWorth() => ConfigurationUtils.ShieldEnemy.PointsWorth;

        public override void ResetEntity()
        {
            base.ResetEntity();

            _shieldCooldownTimer.ResetTimer();
            _shield.ResetShield();
            DeactivateShield();
        }
    }
}