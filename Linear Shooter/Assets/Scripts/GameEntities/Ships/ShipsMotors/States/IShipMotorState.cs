using GameEntities.Ships.Motors.Inputs;
using Utilities;

namespace GameEntities.Ships.Motors.States
{
    public interface IShipMotorState : IState
    {
        /// <summary>
        /// Get's the motor input of the ship.
        /// </summary>
        IShipMotorInput ShipMotorInput { get; }
    }
}