using System;
using System.Collections.Generic;
using GameEntities.Pools;
using GameEntities.PowerUps;
using GameEntities.Ships.Enemies;
using UnityEngine;
using Utilities;
using Utilities.Configuration;

namespace Spawners.TimedSpawners
{
    /// <summary>
    /// Spawns PowerUps in the game.
    /// </summary>
    public class PowerUpSpawner : TimedSpawner
    {
        float _colliderHalfWidth;
        float _colliderHalfHeight;
        Vector3 _spawnLocation;

        [SerializeField]
        GameObject _pools;
        protected Dictionary<IBasePool<PowerUp>, float> ProbabilitiesByPools { get; private set; } = new Dictionary<IBasePool<PowerUp>, float>();
        IBasePool<PowerUp> _defaultPool;

        protected override (float _initialDelay, float _minDelay, float _maxDelay) GetDelays()
        {
            return (
                ConfigurationUtils.PowerUpSpawnerConfig.InitialDelay,
                ConfigurationUtils.PowerUpSpawnerConfig.MinSpawnDelay,
                ConfigurationUtils.PowerUpSpawnerConfig.MaxSpawnDelay
            );
        }

        protected override void Initialize()
        {
            GetPowerUpsDimensionsForSpawning();
            GetAndSortPoolReferenceByProbability();
        }

        private void GetAndSortPoolReferenceByProbability()
        {
            var types = typeof(BasePool<PowerUp>).Assembly.GetTypes();
            var configurationProbabilitiesDictionary = ConfigurationUtils.PowerUpSpawnerConfig.ProbabilitiesByTypesNames;
            (IBasePool<PowerUp> Pool, float Probability) poolWithHighestProbability = (null, float.MinValue);
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface || type.IsEnum)
                    continue;
                if (!CustomMethods.IsSubclassOfRawGeneric(typeof(BasePool<>), type))
                    continue;

                Component possiblePoolReference = _pools.GetComponentInChildren(type);
                if (!(possiblePoolReference is IBasePool<PowerUp>))
                    continue;
                IBasePool<PowerUp> poolReference = (IBasePool<PowerUp>)possiblePoolReference;

                if (configurationProbabilitiesDictionary.ContainsKey(poolReference.PooledEntityTypeName))
                {
                    ProbabilitiesByPools.Add(poolReference, configurationProbabilitiesDictionary[poolReference.PooledEntityTypeName]);
                    if (poolWithHighestProbability.Probability < ProbabilitiesByPools[poolReference])
                        poolWithHighestProbability = (poolReference, ProbabilitiesByPools[poolReference]);
                }
            }
            _defaultPool = poolWithHighestProbability.Pool;
        }

        private void GetPowerUpsDimensionsForSpawning()
        {
            PowerUp powerUp = ExtraLifePowerUpPool.Instance.Get();
            Collider2D collider = powerUp.GetComponent<Collider2D>();

            _colliderHalfWidth = collider.bounds.size.x / 2;
            _colliderHalfHeight = collider.bounds.size.y / 2;

            powerUp.ReturnToPool();
        }

        /// <summary>
        /// Spawns a power up in a random location inside the game.
        /// </summary>
        protected override void Spawn()
        {
            PowerUp powerUp = GetRandomPowerUp();
            powerUp.gameObject.SetActive(false);

            CalculateSpawnPoint();
            bool spawnPointCollidesWithPlayer = false;

            for (int i = 0; i < 5; i++)
            {
                spawnPointCollidesWithPlayer = CustomMethods.CollidesWithPlayer(_spawnLocation, _colliderHalfWidth);
                if (!spawnPointCollidesWithPlayer)
                    break;
                CalculateSpawnPoint();
            }

            if (spawnPointCollidesWithPlayer)
            {
                powerUp.ReturnToPool();
                return;
            }

            powerUp.gameObject.SetActive(true);
            powerUp.transform.position = _spawnLocation;
        }

        private PowerUp GetRandomPowerUp()
        {
            float delta = 0;
            float limit;
            float currentValue;

            foreach (var item in ProbabilitiesByPools)
            {
                limit = item.Value * 100 + delta;
                currentValue = UnityEngine.Random.Range(0, 100f);
                if (currentValue >= delta && currentValue <= limit)
                    return item.Key.Get();
            }
            return _defaultPool.Get();
        }

        private void CalculateSpawnPoint()
        {
            _spawnLocation.x = UnityEngine.Random.Range(ScreenUtils.WorldLeft + _colliderHalfWidth, ScreenUtils.WorldRight - _colliderHalfWidth);
            _spawnLocation.y = UnityEngine.Random.Range(ScreenUtils.WorldBottom + _colliderHalfHeight, ScreenUtils.WorldTop - _colliderHalfHeight);
        }
    }
}