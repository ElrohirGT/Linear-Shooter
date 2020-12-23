using System;
using GameEntities.Ships.Motors.Inputs;
using UnityEngine;
using Utilities;

namespace GameEntities.Ships.Motors.States
{
    public class FreezeMotorState : IShipMotorState
    {
        float _delta = 1f;

        public FreezeMotorState(IShipMotorInput shipMotorInput)
        {
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
            _delta -= Time.deltaTime;
            ShipMotorInput.UpdateInput(Mathf.Clamp(_delta, 0, 1), 0);
        }
    }
}
