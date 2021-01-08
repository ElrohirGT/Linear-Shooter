using System;
using GameEntities.Ships.Enemies;
using GameEntities.Ships.Motors.Inputs;
using UnityEngine;
using Utilities;

namespace GameEntities.Ships.Motors.States
{
    public class MoveTowardsClosestEnemy : IShipMotorState
    {
        readonly Transform _thisTransform;
        ShipEnemy _nearestEnemy = null;

        public MoveTowardsClosestEnemy(Transform thisTransform, IShipMotorInput motorInput)
        {
            ShipMotorInput = motorInput;
            _thisTransform = thisTransform;
        }

        public IShipMotorInput ShipMotorInput { get; }
        public bool AreThereOtherEnemies { get; private set; }
        public bool ReachedTarget { get; private set; }

        public event Action Entered;
        public event Action Exited;

        public void OnEnter() => Entered?.Invoke();

        public void OnExit() => Exited?.Invoke();

        public void Tick()
        {
            if (_nearestEnemy == null)
                _nearestEnemy = GetClosestEnemy();

            if (!AreThereOtherEnemies)
                return;

            float distanceBetweenTarget = Vector2.Distance(_nearestEnemy.transform.position, _thisTransform.position);
            ReachedTarget = distanceBetweenTarget <= 1;

            float thrust = Mathf.Clamp01(distanceBetweenTarget);
            float rotation = CustomMethods.CalculateRotationInput(_thisTransform, _nearestEnemy.transform.position, 0.4f);

            ShipMotorInput.UpdateInput(thrust, rotation);
        }

        private ShipEnemy GetClosestEnemy()
        {
            ShipEnemy[] enemiesInScene = UnityEngine.Object.FindObjectsOfType<ShipEnemy>();
            AreThereOtherEnemies = false;
            (ShipEnemy Enemy, float Distance) nearestEnemy = (null, float.MaxValue);

            foreach (var enemy in enemiesInScene)
            {
                if (enemy.transform.Equals(_thisTransform))
                    continue;

                AreThereOtherEnemies = true;
                float distanceToCheck = Vector2.Distance(_thisTransform.position, enemy.transform.position);
                bool isEnemyNearist = distanceToCheck < nearestEnemy.Distance;

                if (isEnemyNearist)
                    nearestEnemy = (enemy, distanceToCheck);
            }

            if (AreThereOtherEnemies)
                nearestEnemy.Enemy.EntityDied += HandleNearestEnemyDied;

            return nearestEnemy.Enemy;
        }

        private void HandleNearestEnemyDied()
        {
            _nearestEnemy.EntityDied -= HandleNearestEnemyDied;
            _nearestEnemy = null;
        }
    }
}
