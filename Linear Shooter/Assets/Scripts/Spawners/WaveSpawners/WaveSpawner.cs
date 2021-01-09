using System;
using System.Collections.Generic;
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

        #region Timers
        float _spawnDelay;
        Timer _spawnDelayTimer;

        float _waveDelay;
        Timer _waveDelayTimer;
        #endregion

        #region Waves
        WaveGenerator _waveGenerator;
        /// <summary>
        /// Get's the number of remaining entities inside the current wave.
        /// </summary>
        int _currentWaveEntityCount;
        /// <summary>
        /// Get's the number of entities that the spawner has spawned from the current wave.
        /// Not necessarily the same value as <c>_currentWaveEntityCount</c>.
        /// </summary>
        int _currentWaveSpawnedEntities;
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

        bool SpawnerFinishedSpawningEnemies => _currentWaveSpawnedEntities >= _currentWaveTotalEntitiesToSpawn;

        bool AllCurrentWaveEntitiesAreDead => _currentWaveEntityCount <= 0;

        public event Action WaveFinished;

        /*
        .##...##..######..######..##..##...####...#####....####..
        .###.###..##........##....##..##..##..##..##..##..##.....
        .##.#.##..####......##....######..##..##..##..##...####..
        .##...##..##........##....##..##..##..##..##..##......##.
        .##...##..######....##....##..##...####...#####....####..
        .........................................................
        */
        #region Configuration
        protected abstract Dictionary<IBasePool<ShipEnemy>, float> ConstructPoolsAndProbabilitiesDictionary(IBasePool<ShipEnemy>[] poolsGameObject);
        protected abstract float[] GetWaveGeneratorPattern();
        protected abstract float GetWaveGeneratorGrowthValue();
        protected abstract WaveCheckPoint[] GetWaveGeneratorCheckpoints();
        protected abstract float GetSpawnCooldown();
        protected abstract float GetWaveCooldown();
        #endregion

        #region Unity
        void Awake()
        {
            _spawnDelayTimer = gameObject.AddComponent<Timer>();
            _spawnDelayTimer.Finished += HandleSpawnDelayTimerFinished;

            _waveDelayTimer = gameObject.AddComponent<Timer>();
            _waveDelayTimer.Finished += HandleWaveDelayTimerFinished;

            GameManager.AllSpawnersFinished += HandleNextWave;
        }
        void Start() => Initialize();

        public void Initialize()
        {
            float[] pattern = GetWaveGeneratorPattern();
            float growthValue = GetWaveGeneratorGrowthValue();
            WaveCheckPoint[] checkPoints = GetWaveGeneratorCheckpoints();

            _waveGenerator = new WaveGenerator(growthValue, pattern, checkPoints);

            _spawnDelay = GetSpawnCooldown();
            _waveDelay = GetWaveCooldown();

            ProbabilitiesByPools = ConstructPoolsAndProbabilitiesDictionary(GetPoolReferencesFromGameObject());

            HandleWaveDelayTimerFinished();
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
        #endregion

        void HandleNextWave() => _waveDelayTimer.StartTimer(_waveDelay);

        void HandleWaveDelayTimerFinished()
        {
            UpdateInfoFromNextWave();
            HandleSpawnDelayTimerFinished();
        }

        void UpdateInfoFromNextWave()
        {
            _currentWaveEntityCount = 0;
            _currentWaveSpawnedEntities = 0;
            _currentWaveTotalEntitiesToSpawn = _waveGenerator.GetWave();
        }

        void HandleSpawnDelayTimerFinished()
        {
            if (AllCurrentWaveEntitiesAreDead && SpawnerFinishedSpawningEnemies)
            {
                OnCurrentWaveFinished();
                return;
            }

            if (SpawnerFinishedSpawningEnemies)
                return;

            AliveEntity spawnedEntity = Spawn();
            //We need to cast it to Action so the method group is converted to a Delegate.
            if (!EntityHasAlreadyAnEventHandler(spawnedEntity, (Action)HandleEntityDied))
                spawnedEntity.EntityDied += HandleEntityDied;

            _currentWaveSpawnedEntities++;
            _currentWaveEntityCount++;
            _spawnDelayTimer.StartTimer(_spawnDelay);
        }

        protected bool EntityHasAlreadyAnEventHandler(AliveEntity entity, Delegate handlerToCheck)
        {
            if (entity.EntityDied != null)
                foreach (var existingHandler in entity.EntityDied.GetInvocationList())
                    if (existingHandler.Equals(handlerToCheck))
                        return true;
            return false;
        }
        protected void HandleEntityDied()
        {
            _currentWaveEntityCount--;

            if (AllCurrentWaveEntitiesAreDead)
                OnCurrentWaveFinished();
        }

        private void OnCurrentWaveFinished() => WaveFinished?.Invoke();
        /// <summary>
        /// A method that spawns one entity of the wave.
        /// </summary>
        protected abstract AliveEntity Spawn();
    }
}