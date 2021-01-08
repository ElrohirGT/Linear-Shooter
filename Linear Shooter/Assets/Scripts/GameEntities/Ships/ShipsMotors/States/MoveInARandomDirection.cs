using System;
using GameEntities.Ships.Motors.Inputs;
using UnityEngine;

namespace GameEntities.Ships.Motors.States
{
    public class MoveInARandomDirectionState : IShipMotorState
    {
        bool _isRotated = false;
        readonly Transform _thisTransform;

        public MoveInARandomDirectionState(Transform thisTransform, IShipMotorInput shipMotorInput)
        {
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
            if (!_isRotated)
                Rotate();
            ShipMotorInput.UpdateInput(1, 0);
        }

        void Rotate()
        {
            _thisTransform.Rotate(0, 0, UnityEngine.Random.Range(0, 365f));
            _isRotated = true;
        }
    }
}
