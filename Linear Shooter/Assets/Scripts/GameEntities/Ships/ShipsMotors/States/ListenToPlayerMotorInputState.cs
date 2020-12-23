using System;
using GameEntities.Ships.Motors.Inputs;
using GameEntities;

namespace GameEntities.Ships.Motors.States
{
    public class ListenToPlayerMotorInputState : IShipMotorState
    {
        IShipMotorInput _shipMotorInput;

        public IShipMotorInput ShipMotorInput => _shipMotorInput;

        public ListenToPlayerMotorInputState(IShipMotorInput shipMotorInput) => _shipMotorInput = shipMotorInput;

        public event Action Entered;
        public event Action Exited;

        public void OnEnter() => Entered?.Invoke();

        public void OnExit() => Exited?.Invoke();

        public void Tick() => _shipMotorInput.UpdateInput();
    }
}