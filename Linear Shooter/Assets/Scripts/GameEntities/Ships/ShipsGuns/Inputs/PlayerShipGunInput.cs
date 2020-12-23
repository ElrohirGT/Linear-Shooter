using UnityEngine;
using Utilities.Constants;

namespace GameEntities.Ships.Guns.Inputs
{
    public class PlayerShipGunInput : IShipGunInput
    {
        public bool Shoot { get; private set; }

        public void UpdateInput() => Shoot = Input.GetAxis(InputAxisConstants.PLAYER_SHOOT) != 0;

        public void UpdateInput(bool shoot) { }
    }
}