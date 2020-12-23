using System;
using System.Collections.Generic;
using GameEntities;
using GameEntities.Pools;
using GameEntities.Ships.Enemies;
using Utilities;
using Utilities.Configuration;

namespace Spawners.WaveSpawners
{
    public abstract class ShipEnemySpawner : WaveSpawner
    {
        /// <summary>
        /// Get's the pool that has the most probability of spawning.
        /// </summary>
        IBasePool<ShipEnemy> _defaultPool;

        protected override float GetSpawnCooldown() => ConfigurationUtils.EasyEnemiesSpawnerConfig.SpawnCooldownDuration;

        protected override float GetWaveCooldown() => ConfigurationUtils.EasyEnemiesSpawnerConfig.WaveCooldownDuration;

        /// <summary>
        /// Set's the <c>_spawnedEntityPosition</c> vector to a
        /// random world point that is on the borders of the world.
        /// </summary>
        protected void GetRandomWorldPointInBorder()
        {
            bool useTopBorders = UnityEngine.Random.Range(0, 2) > 0;

            if (useTopBorders)
            {
                spawnedEntityPosition.x = UnityEngine.Random.Range(ScreenUtils.WorldLeft, ScreenUtils.WorldRight);
                spawnedEntityPosition.y = UnityEngine.Random.Range(0, 2) > 0 ? ScreenUtils.WorldBottom : ScreenUtils.WorldTop;
                return;
            }
            spawnedEntityPosition.y = UnityEngine.Random.Range(ScreenUtils.WorldBottom, ScreenUtils.WorldTop);
            spawnedEntityPosition.x = UnityEngine.Random.Range(0, 2) > 0 ? ScreenUtils.WorldLeft : ScreenUtils.WorldRight;
        }

        /// <summary>
        /// Checks if the spawner has a listener for the entity died event of the supplied enemy.
        /// </summary>
        /// <param name="enemy">The enemy that will check.</param>
        /// <param name="handlerToCheck">The handler that we want to know if ti already has.</param>
        /// <returns></returns>
        protected bool EnemyHasAlreadyAnEventHandler(AliveEntity enemy, Delegate handlerToCheck)
        {
            if (enemy.EntityDied != null)
                foreach (var existingHandler in enemy.EntityDied.GetInvocationList())
                    if (existingHandler.Equals(handlerToCheck))
                        return true;
            return false;
        }
        /// <summary>
        /// Spawn the entities in random locations around the scene.
        /// </summary>
        protected override void Spawn()
        {
            ShipEnemy newEnemy = GetEnemyFromPools();

            GetRandomWorldPointInBorder();
            newEnemy.transform.position = spawnedEntityPosition;

            //We need to cast it to Action so the method groupd is converted to a Delegate.
            if (!EnemyHasAlreadyAnEventHandler(newEnemy, (Action)HandleEntityDied))
                newEnemy.EntityDied += HandleEntityDied;
        }

        /// <summary>
        /// Get's an enemy from one of the pools based on the probability of each one.
        /// </summary>
        /// <returns>The Ship Enemy to spawn.</returns>
        private ShipEnemy GetEnemyFromPools()
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

        protected override Dictionary<IBasePool<ShipEnemy>, float> ConstructPoolsAndProbabilitiesDictionary(IBasePool<ShipEnemy>[] pools)
        {
            Dictionary<IBasePool<ShipEnemy>, float> probabilities = new Dictionary<IBasePool<ShipEnemy>, float>();

            foreach (var pool in pools)
                if (GetConfigurationTypesDictionary().TryGetValue(pool.PooledEntityTypeName, out float probability))
                    probabilities.Add(pool, probability);

            (IBasePool<ShipEnemy> Pool, float Probability) majorProbabilityPool = (null, float.MinValue);
            foreach (KeyValuePair<IBasePool<ShipEnemy>, float> item in probabilities)
                if (item.Value > majorProbabilityPool.Probability)
                    majorProbabilityPool = (item.Key, item.Value);
            _defaultPool = majorProbabilityPool.Pool;

            return probabilities;
        }

        protected abstract Dictionary<string, float> GetConfigurationTypesDictionary();
    }
}
