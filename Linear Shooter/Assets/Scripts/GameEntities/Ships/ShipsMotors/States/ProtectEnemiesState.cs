using System;
using GameEntities.Ships.Enemies;
using GameEntities.Ships.Motors.Inputs;
using UnityEngine;
using Utilities;
using Utilities.Constants;

namespace GameEntities.Ships.Motors.States
{
    public class ProtectEnemiesState : IShipMotorState
    {
        const int MAX_ENEMIES_TO_CHECK = 10;
        readonly float _protectiveRadius;
        Collider2D[] _enemiesInsideProtectiveRadius = new Collider2D[MAX_ENEMIES_TO_CHECK];
        readonly Transform _thisTransform;

        public bool IsThereEnemiesInRadius { get; private set; }

        public ProtectEnemiesState(float protectiveRadius, Transform thisTransform, IShipMotorInput shipMotorInput)
        {
            _protectiveRadius = protectiveRadius;
            _thisTransform = thisTransform;
            ShipMotorInput = shipMotorInput;
        }

        public IShipMotorInput ShipMotorInput { get; }

        public event Action Entered;
        public event Action Exited;

        public void OnEnter() => Entered?.Invoke();

        public void OnExit() => Exited?.Invoke();

        public void Tick()
        {
            Transform target = GetFarestTransform();

            if (!IsThereEnemiesInRadius)
                return;

            float thrust = Vector2.Distance(_thisTransform.position, target.position) / _protectiveRadius;
            float rotation = CustomMethods.CalculateRotationInput(_thisTransform, target.position);

            ShipMotorInput.UpdateInput(thrust, rotation);
        }

        private Transform GetFarestTransform()
        {
            int layerMask = (int)LayerMasks.Enemies;
            int max = Physics2D.OverlapCircleNonAlloc(_thisTransform.position, _protectiveRadius, _enemiesInsideProtectiveRadius, layerMask);
            (Transform Transform, float Distance) farestTransfom = (_thisTransform, 0);
            IsThereEnemiesInRadius = false;

            for (int i = 0; i < max; i++)
            {
                Collider2D currentCollider = _enemiesInsideProtectiveRadius[i];
                bool colliderIsNotTagged = !currentCollider.CompareTag(TagsConstants.ENEMY);
                bool colliderEntityIsThisEntity = currentCollider.transform.Equals(_thisTransform);

                if (colliderIsNotTagged)
                    continue;

                if (colliderEntityIsThisEntity)
                    continue;

                IsThereEnemiesInRadius = true;

                float distance = Vector2.Distance(currentCollider.transform.position, _thisTransform.position);
                if (farestTransfom.Distance < distance)
                    farestTransfom = (currentCollider.transform, distance);
            }
            return farestTransfom.Transform;
        }
    }
}
