using UnityEngine;
using Utilities;
using Utilities.Configuration;

namespace Spawners.TimedSpawners
{
    /// <summary>
    /// Represents a spawner that spawns according to a random time from the minDelay and maxDelay inclusive.
    /// </summary>
    public abstract class TimedSpawner : MonoBehaviour
    {
        /// <summary>
        /// Get's the initial delay of the spawner.
        /// </summary>
        float _initialDelay;

        /// <summary>
        /// Get's the minimum delay that needs to wait before spawning
        /// </summary>
        float _minDelay;

        /// <summary>
        /// Get's the maximmum delay to wait to spawn.
        /// </summary>
        float _maxDelay;

        /// <summary>
        /// The timer that controls the spawning.
        /// </summary>
        Timer _spawnTimer;

        void Awake()
        {
            ConfigurationUtils.Initialize();

            (_initialDelay, _minDelay, _maxDelay) = GetDelays();

            _spawnTimer = gameObject.AddComponent<Timer>();
            _spawnTimer.Finished += HandleSpawnTimerFinished;
        }

        protected abstract (float _initialDelay, float _minDelay, float _maxDelay) GetDelays();

        void Start()
        {
            Initialize();
            _spawnTimer.StartTimer(_initialDelay);
        }

        /// <summary>
        /// If the implementation needs to to something before initializing, this method is called in the Start.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Calls the spawn method.
        /// </summary>
        private void HandleSpawnTimerFinished()
        {
            Spawn();
            _spawnTimer.StartTimer(UnityEngine.Random.Range(_minDelay, _maxDelay));
        }

        /// <summary>
        /// The implementation must spawn something from a pool.
        /// </summary>
        protected abstract void Spawn();

    }
}