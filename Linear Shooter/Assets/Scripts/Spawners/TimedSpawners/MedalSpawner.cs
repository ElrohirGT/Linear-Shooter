using Utilities;
using UnityEngine;
using Utilities.Configuration;
using Utilities.Constants;
using GameEntities;
using GameEntities.Pools;

namespace Spawners.TimedSpawners
{
    /// <summary>
    /// Spawns medals into the game.
    /// </summary>
    public class MedalSpawner : TimedSpawner
    {
        float _colliderHalfHeight;
        float _colliderHalfWidth;

        Vector3 _spawnEntityPosition = new Vector3();

        protected override (float _initialDelay, float _minDelay, float _maxDelay) GetDelays()
        {
            return (
                ConfigurationUtils.MedalSpawnerConfig.InitialDelay,
                ConfigurationUtils.MedalSpawnerConfig.MinSpawnDelay,
                ConfigurationUtils.MedalSpawnerConfig.MaxSpawnDelay
            );
        }

        /// <summary>
        /// Spawns a medal in a random location in the game.
        /// </summary>
        protected override void Spawn()
        {
            CalculateSpawnPosition();
            bool collidesWithPlayer = false;

            //It will try 5 times to spawn the medal in a random location where it doesn't collides with the player.
            for (int i = 0; i < 5; i++)
            {
                collidesWithPlayer = CustomMethods.CollidesWithPlayer(_spawnEntityPosition, _colliderHalfWidth, 15);
                if (!collidesWithPlayer)
                    break;
                CalculateSpawnPosition();
            }

            if (collidesWithPlayer)
                return;

            Medal newMedal = MedalPool.Instance.Get();
            newMedal.transform.position = _spawnEntityPosition;
        }

        private void CalculateSpawnPosition()
        {
            //_spawnEntityPosition = Player.Position;

            _spawnEntityPosition.x = Random.Range(ScreenUtils.WorldLeft + _colliderHalfWidth, ScreenUtils.WorldRight - _colliderHalfWidth);
            _spawnEntityPosition.y = Random.Range(ScreenUtils.WorldBottom + _colliderHalfHeight, ScreenUtils.WorldTop - _colliderHalfHeight);
        }

        protected override void Initialize()
        {
            Medal newMedal = MedalPool.Instance.Get();
            Collider2D collider = newMedal.GetComponent<Collider2D>();

            _colliderHalfWidth = collider.bounds.size.x / 2;
            _colliderHalfHeight = collider.bounds.size.y / 2;

            newMedal.ReturnToPool();
        }
    }
}