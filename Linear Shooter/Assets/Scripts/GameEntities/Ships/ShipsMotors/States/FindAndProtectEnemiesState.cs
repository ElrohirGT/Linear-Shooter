using System;
using GameEntities.Shields;
using GameEntities.Ships.Motors.Inputs;
using UnityEngine;
using Utilities;
using Utilities.Constants;

namespace GameEntities.Ships.Motors.States
{
    public class FindAndProtectEnemiesState : IShipMotorState
    {
        const int MAX_ENTITIES_TO_CHECK = 20;

        readonly Transform _thisTransform;
        readonly float _protectiveRadius;
        Collider2D[] _entitiesInProtectiveRadius = new Collider2D[MAX_ENTITIES_TO_CHECK];
        Transform _targetToMoveTo;

        bool _IsMovingInRandomDirection;

        public FindAndProtectEnemiesState(Transform shipTransform, float protectiveRadius, IShipMotorInput shipMotorInput)
        {
            _thisTransform = shipTransform;
            _protectiveRadius = protectiveRadius;
            ShipMotorInput = shipMotorInput;
        }

        public IShipMotorInput ShipMotorInput { get; private set; }

        public event Action Entered;
        public event Action Exited;

        public void OnEnter()
        {
            Entered?.Invoke();
        }

        public void OnExit()
        {
            Exited?.Invoke();
        }

        public void Tick()
        {
            if (EnemiesInProtectiveArea())
                MoveTowardsFarestProtectedEnemy();
            else
                MoveInARandomDirection();
        }

        private bool EnemiesInProtectiveArea()
        {
            int max = Physics2D.OverlapCircleNonAlloc(
                _thisTransform.position,
                _protectiveRadius,
                _entitiesInProtectiveRadius,
                (int)LayerMasks.Enemies
            );
            int numberOfEnemiesInProtectiveArea = 0;

            (Transform transform, float distance) farestTransform = (_thisTransform, 0);

            for (int i = 0; i < max; i++)
            {
                Collider2D entityCollider = _entitiesInProtectiveRadius[i];

                if (!entityCollider.CompareTag(TagsConstants.ENEMY))
                    continue;
                if (entityCollider.gameObject.TryGetComponent(out Shield _))
                    continue;
                if (_thisTransform.position.Equals(entityCollider.transform.position))
                    continue;

                numberOfEnemiesInProtectiveArea++;

                float distance = Vector2.Distance(_thisTransform.position, entityCollider.transform.position);
                if (distance > farestTransform.distance)
                    farestTransform = (entityCollider.transform, distance);
            }

            _targetToMoveTo = farestTransform.transform;

            return numberOfEnemiesInProtectiveArea != 0;
        }

        private void MoveTowardsFarestProtectedEnemy()
        {
            Debug.Log($"Moving towards: {_targetToMoveTo.position}");
            _IsMovingInRandomDirection = false;

            //The thrust will be 1 when the enemy is at the border of the protective radius
            //and it will decrease the closer the enemy is.
            float thrust = Vector2.Distance(_thisTransform.position, _targetToMoveTo.position) / _protectiveRadius;
            float rotation = Utilities.CustomMethods.CalculateRotationInput(_thisTransform, _targetToMoveTo.position, 0.1f);

            ShipMotorInput.UpdateInput(thrust, rotation);
        }

        private void MoveInARandomDirection()
        {
            if (!_IsMovingInRandomDirection)
            {
                _thisTransform.Rotate(0, 0, UnityEngine.Random.Range(0, 360));
                _IsMovingInRandomDirection = true;
            }

            ShipMotorInput.UpdateInput(1, 0);
        }
    }
}
