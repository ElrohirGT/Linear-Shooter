using UnityEngine;

namespace GameEntities.Ships.Guns.Inputs
{
    public interface IShipGunInput : IShipInput
    {
        /// <summary>
        /// Represent's the input to shoot.
        /// </summary>
        bool Shoot { get; }
        /// <summary>
        /// Updates the input according to the value supplied. This method is here for AI.
        /// </summary>
        /// <param name="shoot">The new value.</param>
        void UpdateInput(bool shoot);
    }
}