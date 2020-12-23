using GameEntities.Bullets;
using GameEntities.Ships.Guns.Inputs;
using Utilities;

namespace GameEntities.Ships.Guns
{
    public interface IShipGun<out T> where T : Bullet
    {
        /// <summary>
        /// Initializes the ShipGun with the specified input, settings and state machine.
        /// </summary>
        /// <param name="shipGunInput">The input manager for this gun.</param>
        /// <param name="shipGunSettings">The settings for this gun.</param>
        /// <param name="stateMachine">The state manager for this gun.</param>
        void Initialize(IShipGunInput shipGunInput, ShipGunSettings shipGunSettings, StateMachine stateMachine);
    }
}