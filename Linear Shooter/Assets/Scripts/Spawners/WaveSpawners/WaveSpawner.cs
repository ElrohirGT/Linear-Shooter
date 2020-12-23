using System;
using System.Collections.Generic;
using System.Linq;
using GameEntities;
using GameEntities.Pools;
using GameEntities.Ships.Enemies;
using UnityEngine;
using Utilities;
using Utilities.Configuration;

namespace Spawners.WaveSpawners
{
    /// <summary>
    /// Represents a spawner that spawns things by waves without taking into account time.
    /// </summary>
    public abstract class WaveSpawner : MonoBehaviour
    {
        /*
        .######..######..######..##......#####....####..
        .##........##....##......##......##..##..##.....
        .####......##....####....##......##..##...####..
        .##........##....##......##......##..##......##.
        .##......######..######..######..#####....####..
        ................................................
        */

        protected Vector3 spawnedEntityPosition = new Vector3();

        /// <summary>
        /// Used to enable or disable the spawner.
        /// </summary>
        bool _isActive = false;

        #region Timers
        /// <summary>
        /// Get's a delay before spawning a new enemy in the current wave.
        /// </summary>
        float _spawnCooldownDuration;

        /// <summary>
        /// Get's the timer that controls the delay between spawns of entities in the same wave.
        /// </summary>
        Timer _spawnCooldownTimer;

        /// <summary>
        /// Get's the delay between waves.
        /// </summary>
        float _waveCooldownDuration;

        /// <summary>
        /// Get's the timer that controls the delays between waves.
        /// </summary>
        Timer _waveCooldownTimer;
        #endregion

        #region Waves
        /// <summary>
        /// Get's the number of remaining entities inside the current wave.
        /// </summary>
        int _currentWaveEntitiesCount;
        /// <summary>
        /// Get's the number of entities that the spawner has spawned from the current wave.
        /// </summary>
        int _currentWaveSpawnedEntities;
        /// <summary>
        /// Creates all the waves that this spawner will spawn.
        /// </summary>
        WaveGenerator _waveGenerator;
        /// <summary>
        /// Get's how many entities this wave has needs to spawn.
        /// </summary>
        int _currentWaveTotalEntitiesToSpawn;
        #endregion

        #region Support to spawn by probabilities
        [SerializeField]
        GameObject _pools;
        protected Dictionary<IBasePool<ShipEnemy>, float> ProbabilitiesByPools { get; private set; }
        #endregion

        /*
        .######..##..##..######..##..##..######...####..
        .##......##..##..##......###.##....##....##.....
        .####....##..##..####....##.###....##.....####..
        .##.......####...##......##..##....##........##.
        .######....##....######..##..##....##.....####..
        ................................................
        */
        /// <summary>
        /// An event that fires when the current wave has finished.
        /// </summary>
        public Action WaveFinished;

        /*
        .##...##..######..######..##..##...####...#####....####..
        .###.###..##........##....##..##..##..##..##..##..##.....
        .##.#.##..####......##....######..##..##..##..##...####..
        .##...##..##........##....##..##..##..##..##..##......##.
        .##...##..######....##....##..##...####...#####....####..
        .........................................................
        */
        #region Unity
        void Awake()
        {
            _spawnCooldownTimer = gameObject.AddComponent<Timer>();
            _spawnCooldownTimer.Finished += HandleDelayBetweenSpawnsTimerFinished;

            _waveCooldownTimer = gameObject.AddComponent<Timer>();
            _waveCooldownTimer.Finished += HandleDelayBetweenWavesTimerFinished;

            GameManager.AllSpawnersFinished += HandleNextWave;
        }
        void Start() => Initialize();

        public void Initialize()
        {
            float[] pattern = GetWaveGeneratorPattern();
            float growthValue = GetWaveGeneratorGrowthValue();
            WaveCheckPoint[] checkPoints = GetWaveGeneratorCheckpoints();

            _waveGenerator = new WaveGenerator(growthValue, pattern, checkPoints);
            _currentWaveTotalEntitiesToSpawn = _waveGenerator.GetWave();

            _spawnCooldownDuration = GetSpawnCooldown();
            _waveCooldownDuration = GetWaveCooldown();

            ProbabilitiesByPools = ConstructPoolsAndProbabilitiesDictionary(GetPoolReferencesFromGameObject());
            _isActive = true;

            SpawnWave();
        }

        IBasePool<ShipEnemy>[] GetPoolReferencesFromGameObject()
        {
            var types = typeof(BasePool<ShipEnemy>).Assembly.GetTypes();
            List<Type> poolsTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface || type.IsEnum)
                    continue;
                if (CustomMethods.IsSubclassOfRawGeneric(typeof(BasePool<>), type))
                    poolsTypes.Add(type);
            }
            List<IBasePool<ShipEnemy>> poolsReferences = new List<IBasePool<ShipEnemy>>();
            foreach (var poolType in poolsTypes)
            {
                if (_pools.GetComponentInChildren(poolType) is IBasePool<ShipEnemy> poolReference)
                    poolsReferences.Add(poolReference);
            }
            return poolsReferences.ToArray();
        }

        protected abstract Dictionary<IBasePool<ShipEnemy>, float> ConstructPoolsAndProbabilitiesDictionary(IBasePool<ShipEnemy>[] poolsGameObject);
        protected abstract float[] GetWaveGeneratorPattern();
        protected abstract float GetWaveGeneratorGrowthValue();
        protected abstract WaveCheckPoint[] GetWaveGeneratorCheckpoints();
        protected abstract float GetSpawnCooldown();
        protected abstract float GetWaveCooldown();
        #endregion

        void HandleNextWave()
        {
            _currentWaveTotalEntitiesToSpawn = _waveGenerator.GetWave();
            _currentWaveEntitiesCount = 0;
            _currentWaveSpawnedEntities = 0;
            _waveCooldownTimer.StartTimer(_waveCooldownDuration);
        }

        /// <summary>
        /// Start's the spawns cicle.
        /// </summary>
        void SpawnWave() => HandleDelayBetweenSpawnsTimerFinished();

        /// <summary>
        /// Handles the delay between waves of entities.
        /// </summary>
        void HandleDelayBetweenWavesTimerFinished() => SpawnWave();

        /// <summary>
        /// Handles the delay between the spawn of every enemy in the same wave.
        /// </summary>
        void HandleDelayBetweenSpawnsTimerFinished()
        {
            //This wave the spawners doesn't spawns anything so it just says it finished.
            if (_currentWaveTotalEntitiesToSpawn == 0)
            {
                OnCurrentWaveFinished();
                return;
            }

            //The spawner already spawned everything it needed to.
            if (_currentWaveSpawnedEntities >= _currentWaveTotalEntitiesToSpawn)
                return;

            //The spawner is not active.
            if (!_isActive)
                return;

            Spawn();

            _currentWaveSpawnedEntities++;
            _currentWaveEntitiesCount++;
            _spawnCooldownTimer.StartTimer(_spawnCooldownDuration);
        }

        /// <summary>
        /// Updates the current entity count and possibly announces the wave has ended.
        /// </summary>
        protected void HandleEntityDied()
        {
            _currentWaveEntitiesCount--;

            if (_currentWaveEntitiesCount <= 0)
                OnCurrentWaveFinished();
        }

        /// <summary>
        /// Invokes the wave finished event.
        /// </summary>
        private void OnCurrentWaveFinished() => WaveFinished?.Invoke();

        /// <summary>
        /// A method that spawns one entity of the wave.
        /// </summary>
        protected abstract void Spawn();
    }
}