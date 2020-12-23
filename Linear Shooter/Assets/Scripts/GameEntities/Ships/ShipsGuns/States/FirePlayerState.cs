using System;
using GameEntities.Ships.Guns.Inputs;
using UnityEngine;

namespace GameEntities.Ships.Guns.States
{
    public class FirePlayerState : IShipGunState
    {
        IShipGunInput _shipGunInput;

        public IShipGunInput ShipGunInput => _shipGunInput;

        public event Action Entered;
        public event Action Exited;

        public FirePlayerState(IShipGunInput shipGunInput)
        {
            _shipGunInput = shipGunInput;
        }

        public void OnEnter() => Entered?.Invoke();

        public void OnExit() => Exited?.Invoke();

        public void Tick() => _shipGunInput.UpdateInput(true);
    }
}