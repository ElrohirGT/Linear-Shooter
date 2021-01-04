using System;
using Events;
using UnityEngine;
using Utilities;
using Utilities.Configuration;

namespace GameEntities
{
    /// <summary>
    /// Represents an alive entity in the game world.
    /// </summary>
    public abstract class AliveEntity : MonoBehaviour
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
        /// Manages the states from this entities.
        /// </summary>
        StateMachine _entityStateMachine;

        #region Life
        /// <summary>
        /// Get's the max hitpoints this entity can have.
        /// </summary>
        float _maxHitpoints;

        /// <summary>
        /// Get's the remaining hitpoints this entity has.
        /// </summary>
        float _currentHitpoints;

        #endregion

        #region Collision damage support
        /// <summary>
        /// Get's the base damage this entity causes when colliding with other entities.
        /// </summary>
        float _baseDamage;

        /// <summary>
        /// Get's whether the player ship can receive damage or not
        /// </summary>
        bool _canReceiveDamage = true;

        /// <summary>
        /// The timer that controls the timeout of the ship in the game.
        /// </summary>
        Timer _damageCooldownTimer;

        /// <summary>
        /// Get's how much time the ship has on break before being able to receive damage again.
        /// By default is 0.5.
        /// </summary>
        float _damageCooldownDuration = 0.5f;
        #endregion

        /// <summary>
        /// Get's the base damage this entity causes when colliding with other entities.
        /// </summary>
        public float EntityBaseDamage => _baseDamage;

        /// <summary>
        /// Get's the remaining hitpoints of this entity
        /// </summary>
        public float RemainingHitpoints => _currentHitpoints;

        /// <summary>
        /// Get's this entity state machine.
        /// </summary>
        protected StateMachine EntityStateMachine => _entityStateMachine;

        protected bool CanReceiveDamage => _canReceiveDamage;

        protected float MaxHitpoints => _maxHitpoints;
        protected float CurrentHitpoints => _currentHitpoints;

        /*
        .######..##..##..######..##..##..######...####..
        .##......##..##..##......###.##....##....##.....
        .####....##..##..####....##.###....##.....####..
        .##.......####...##......##..##....##........##.
        .######....##....######..##..##....##.....####..
        ................................................
        */
        /// <summary>
        /// This event is invoked when the entity takes damage.
        /// </summary>
        public Action<EntityHitpointsChanged> EntityTookDamage;
        /// <summary>
        /// This event is invoked when the entity heals.
        /// </summary>
        public Action<EntityHitpointsChanged> EntityHealed;

        /// <summary>
        /// This event is invoked when the entity doesn't have any remaining hitpoints.
        /// </summary>
        public Action EntityDied;

        /// <summary>
        /// This event is invoked when the damage cooldown timer has finished, so the entity can receive damage again.
        /// </summary>
        protected Action DamageCooldownTimerFinished;

        /*
        .##...##..######..######..##..##...####...#####....####..
        .###.###..##........##....##..##..##..##..##..##..##.....
        .##.#.##..####......##....######..##..##..##..##...####..
        .##...##..##........##....##..##..##..##..##..##......##.
        .##...##..######....##....##..##...####...#####....####..
        .........................................................
        */
        protected virtual void Awake()
        {
            ConfigurationUtils.Initialize();

            _damageCooldownTimer = gameObject.AddComponent<Timer>();
            _damageCooldownTimer.Finished += HandleDamageCooldownTimerFinished;

            InitializeEntity();
        }

        /// <summary>
        /// Initializes the entity, IPoolable entities need to call this method in their ResetEntity method.
        /// </summary>
        protected void InitializeEntity() => (_maxHitpoints, _currentHitpoints, _baseDamage, _damageCooldownDuration) = GetInitializationValues();

        /// <summary>
        /// Get's the values to initialize the entity.
        /// </summary>
        /// <returns>The maxHitpoints, the currentHitpoints, baseDamage and damageCooldownDuration this entity will use.</returns>
        protected abstract (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues();

        #region EventHandling
        private void HandleDamageCooldownTimerFinished()
        {
            DamageCooldownTimerFinished?.Invoke();
            _canReceiveDamage = true;
        }

        /// <summary>
        /// Invokes the TookDamage event,
        /// if the damage taken is negative or 0, the entity doesn't take any damage.
        /// </summary>
        protected void OnEntityTookDamage(float damageTaken)
        {
            if (!_canReceiveDamage || damageTaken <= 0)
                return;

            _canReceiveDamage = false;
            TakeDamage(damageTaken);

            EntityTookDamage?.Invoke(new EntityHitpointsChanged(_currentHitpoints, _maxHitpoints));
            _damageCooldownTimer.StartTimer(_damageCooldownDuration);

            if (_currentHitpoints <= 0)
                OnEntityDied();
        }
        protected void OnEntityHeals(float healAmount)
        {
            if (healAmount > 0)
            {
                HealEntity(healAmount);
                EntityHealed?.Invoke(new EntityHitpointsChanged(_currentHitpoints, _maxHitpoints));
            }
        }

        /// <summary>
        /// Invokes the died event of this entity.
        /// </summary>
        void OnEntityDied() => EntityDied?.Invoke();
        #endregion

        protected void MakeInvincible() => _canReceiveDamage = false;

        protected void MakeVincible() => _canReceiveDamage = true;

        /// <summary>
        /// Reduces the hitpoints of the Entity by the specified damage.
        /// </summary>
        /// <param name="damage">The damage to take.</param>
        void TakeDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "The damage to take must be a positive number!");
            _currentHitpoints -= damage;
        }
        /// <summary>
        /// Adds the <paramref name="healAmount"/> to the entity hitpoints.
        /// </summary>
        /// <param name="healAmount">The amount to heal.</param>
        void HealEntity(float healAmount)
        {
            if (healAmount < 0)
                throw new ArgumentOutOfRangeException(nameof(healAmount), "The heal amount must be a positive number!");
            _currentHitpoints += healAmount;
            if (_currentHitpoints > _maxHitpoints)
                _maxHitpoints = _currentHitpoints;
        }
    }
}