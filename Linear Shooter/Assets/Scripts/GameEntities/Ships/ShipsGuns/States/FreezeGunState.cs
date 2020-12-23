using System;
using GameEntities.Ships.Guns.Inputs;
using Utilities;

namespace GameEntities.Ships.Guns.States
{
    internal class FreezeGunState : IShipGunState
    {
        public FreezeGunState(IShipGunInput shipGunInput)
        {
            ShipGunInput = shipGunInput;
        }

        public IShipGunInput ShipGunInput { get; }

        public event Action Entered;
        public event Action Exited;

        public void OnEnter() => Entered?.Invoke();

        public void OnExit() => Exited?.Invoke();

        public void Tick() => ShipGunInput.UpdateInput(false);
    }
}