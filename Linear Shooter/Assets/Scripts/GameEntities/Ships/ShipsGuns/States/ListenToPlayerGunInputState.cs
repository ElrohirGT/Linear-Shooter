using System;
using GameEntities.Ships.Guns.Inputs;

namespace GameEntities.Ships.Guns.States
{
    public class ListenToPlayerGunInputState : IShipGunState
    {
        readonly IShipGunInput _shipGunInput;

        public IShipGunInput ShipGunInput => _shipGunInput;

        public ListenToPlayerGunInputState(IShipGunInput shipGunInput) => _shipGunInput = shipGunInput;

        public event Action Entered;
        public event Action Exited;

        public void OnEnter() => Entered?.Invoke();

        public void OnExit() => Exited?.Invoke();

        public void Tick() => ShipGunInput.UpdateInput();
    }
}
