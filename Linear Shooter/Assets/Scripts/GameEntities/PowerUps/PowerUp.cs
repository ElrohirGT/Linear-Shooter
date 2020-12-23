using UnityEngine;
using GameEntities.Pools;
using Utilities;
using Utilities.Constants;
using Utilities.Configuration;

namespace GameEntities.PowerUps
{
    /// <summary>
    /// Represents a power up inside the game.
    /// </summary>
    public abstract class PowerUp : MonoBehaviour, IPoolableEntity
    {
        float _lifeDuration;
        Timer _lifeTimer;
        FadesAway _fadeAwayComponent;

        void Awake()
        {
            ConfigurationUtils.Initialize();

            _lifeDuration = GetLifeDuration();
            _lifeTimer = gameObject.AddComponent<Timer>();
            _lifeTimer.Finished += HandleLifeTimerFinished;

            _fadeAwayComponent = gameObject.AddComponent<FadesAway>().Initalize(GetComponent<SpriteRenderer>(), _lifeTimer);
        }

        protected abstract float GetLifeDuration();

        void OnEnable() => _lifeTimer.StartTimer(_lifeDuration);

        void HandleLifeTimerFinished() => ReturnToPool();

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TagsConstants.PLAYER))
            {
                OnPlayerPickedUp();
                ReturnToPool();
            }
        }
        protected abstract void OnPlayerPickedUp();

        public abstract void ReturnToPool();

        public void ResetEntity()
        {
            _lifeTimer.ResetTimer();
            _fadeAwayComponent.ResetColor();
        }
    }
}