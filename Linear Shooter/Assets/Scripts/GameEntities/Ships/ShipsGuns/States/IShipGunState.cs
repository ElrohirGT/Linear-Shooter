using GameEntities.Ships.Guns.Inputs;
using Utilities;

namespace GameEntities.Ships.Guns.States
{
    public interface IShipGunState : IState
    {
        /// <summary>
        /// Get's the gun input of the ship.
        /// </summary>
        IShipGunInput ShipGunInput { get; }
    }
}
