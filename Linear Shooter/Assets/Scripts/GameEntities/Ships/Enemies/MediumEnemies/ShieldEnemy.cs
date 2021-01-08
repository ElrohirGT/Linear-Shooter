using System;
using System.Collections;
using GameEntities.Pools;
using GameEntities.Shields;
using GameEntities.Ships.Motors;
using GameEntities.Ships.Motors.States;
using UnityEngine;
using Utilities;
using Utilities.Configuration;
using Utilities.Constants;

namespace GameEntities.Ships.Enemies.MediumEnemies
{
    public class ShieldEnemy : ShipEnemy
    {
        [SerializeField]
        Shield _shield;

        Animator _animator;
        const string IDLE_ANIMATION = "ShieldEnemy_IDLE";
        const string ACTIVATE_SHIELD_ANIMATION = "ShieldEnemy_ActivateShield";
        const string SHIELD_ACTIVE = "ShieldEnemy_ShieldActive";
        const string DEACTIVATE_SHIELD_ANIMATION = "ShieldEnemy_DeactivateShield";

        Timer _shieldCooldownTimer;
        float _shieldCooldown;
        bool IsShieldActivated => _shield.isActiveAndEnabled;

        IState _fleeFromPlayerState;

        public override void ReturnToPool() => ShieldEnemyPool.Instance.ReturnToPool(this);

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();

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
            ActivateShieldWithoutAnimation();
            Collider2D shieldCollider = _shield.GetComponent<Collider2D>();
            float shieldColliderRadius = shieldCollider.bounds.size.x / 2;

            var protectState = new ProtectEnemiesState(shieldColliderRadius, transform, ShipMotorInput);
            var moveTowardsClosestEnemyState = new MoveTowardsClosestEnemy(transform, ShipMotorInput);
            var moveInARandomDirectionState = new MoveInARandomDirectionState(transform, ShipMotorInput);

            ShipMotorStateMachine.AddAnyTransition(_fleeFromPlayerState, () => !IsShieldActivated);

            ShipMotorStateMachine.AddTransition(_fleeFromPlayerState, protectState, () => IsShieldActivated);
            ShipMotorStateMachine.AddTransition(protectState, moveTowardsClosestEnemyState, () => !protectState.IsThereEnemiesInRadius);
            ShipMotorStateMachine.AddTransition(moveTowardsClosestEnemyState, moveInARandomDirectionState, () => !moveTowardsClosestEnemyState.AreThereOtherEnemies);

            ShipMotorStateMachine.AddTransition(moveTowardsClosestEnemyState, protectState, () => moveTowardsClosestEnemyState.ReachedTarget);

            DeactivateShieldWithoutAnimation();
        }
        private void HandleShieldCooldownTimerFinished() => ActivateShieldWithAnimation();
        private void HandleShieldBroke() => DeactivateShieldWithAnimation();

        void ActivateShieldWithAnimation()
        {
            _shield.gameObject.SetActive(true);
            CustomMethods.PlayAnimation(_animator, ACTIVATE_SHIELD_ANIMATION);
            StartCoroutine(ActivateShieldAfterAnimation());
        }

        private IEnumerator ActivateShieldAfterAnimation()
        {
            yield return null;

            float animationDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationDuration);

            ActivateShieldWithoutAnimation();
        }

        void ActivateShieldWithoutAnimation()
        {
            _shield.gameObject.SetActive(true);
            MakeInvincible();
            CustomMethods.PlayAnimation(_animator, SHIELD_ACTIVE);
        }

        void DeactivateShieldWithoutAnimation()
        {
            _shield.gameObject.SetActive(false);
            MakeVincible();
            _shieldCooldownTimer.StartTimer(_shieldCooldown);
            CustomMethods.PlayAnimation(_animator, IDLE_ANIMATION);
        }
        void DeactivateShieldWithAnimation()
        {
            CustomMethods.PlayAnimation(_animator, DEACTIVATE_SHIELD_ANIMATION);
            StartCoroutine(DeactivateShieldAfterAnimation());
        }
        IEnumerator DeactivateShieldAfterAnimation()
        {
            //wait for a frame so the animator state machine updates
            yield return null;

            float animationDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationDuration);

            DeactivateShieldWithoutAnimation();
        }

        protected override ShipMotorSettings CreateMotorSettings() => new ShipMotorSettings(
                ConfigurationUtils.ShieldEnemy.RotationSpeed,
                ConfigurationUtils.ShieldEnemy.ThrustAmount
            );

        protected override StateMachine CreateShipMotorStateMachine()
        {
            _fleeFromPlayerState = gameObject.AddComponent<FleeFromPlayerState>().Initialize(ShipMotorInput, ConfigurationUtils.ShieldEnemy.RotationCooldown);
            return new StateMachine(_fleeFromPlayerState);
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
            DeactivateShieldWithAnimation();
        }
    }
}