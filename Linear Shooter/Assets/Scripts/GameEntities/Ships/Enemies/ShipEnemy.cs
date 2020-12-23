using System;
using Events;
using GameEntities.Bullets;
using GameEntities.Ships.Motors.Inputs;
using GameEntities.Pools;
using Utilities.Constants;
using UnityEngine;
using GameEntities.Ships.Motors.States;
using Utilities;
using GameEntities.PowerUps;

namespace GameEntities.Ships.Enemies
{
    /// <summary>
    /// Represent's an enemy in the game that could be called a ship.
    /// </summary>
    public abstract class ShipEnemy : Ship, IPoolableEntity
    {
        /// <summary>
        /// Get's the point's that this enemy is worth when it dies.
        /// </summary>
        float _pointsWorth;

        [SerializeField]
        HealthBar _healthBar;

        /// <summary>
        /// Event that fires when an enemy dies, it's static so the HUD and the Player can track this event.
        /// And it's because it's static that all enemies share this event.
        /// </summary>
        public static Action<EnemyDiedEventInfo> EnemyDied;

        protected override void Awake()
        {
            base.Awake();

            _pointsWorth = GetPointsWorth();

            EntityDied += HandleShipEnemyDiedEvent;
            EntityTookDamage += HandleEntityTookDamageEvent;

            IState normalState = ShipMotorStateMachine.CurrentState;
            IState freezeState = new FreezeMotorState(ShipMotorInput);

            ShipMotorStateMachine.AddAnyTransition(freezeState, () => EffectManager.Instance.IsFreezerEffectActivated);
            ShipMotorStateMachine.AddTransition(freezeState, normalState, () => !EffectManager.Instance.IsFreezerEffectActivated);
        }

        private void HandleEntityTookDamageEvent(EntityHitpointsChanged obj)
        {
            _healthBar.gameObject.SetActive(true);
            _healthBar.HealthChanged(obj.RemainingHitpoints);
        }

        protected abstract float GetPointsWorth();

        protected override IShipMotorInput CreateMotorInput() => new AIMotorInput();

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(TagsConstants.PLAYER))
                OnEntityTookDamage(collision.gameObject.GetComponent<AliveEntity>().EntityBaseDamage);

            else if (collision.gameObject.CompareTag(TagsConstants.PLAYER_BULLET))
            {
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();
                OnEntityTookDamage(bullet.Damage);
                bullet.ReturnToPool();
            }
        }

        void HandleShipEnemyDiedEvent()
        {
            EnemyDied?.Invoke(new EnemyDiedEventInfo(_pointsWorth));
            ReturnToPool();
        }

        public abstract void ReturnToPool();
        public virtual void ResetEntity()
        {
            InitializeEntity();
            _healthBar.Initialize(MaxHitpoints, CurrentHitpoints);
            _healthBar.gameObject.SetActive(false);
        }
    }
}