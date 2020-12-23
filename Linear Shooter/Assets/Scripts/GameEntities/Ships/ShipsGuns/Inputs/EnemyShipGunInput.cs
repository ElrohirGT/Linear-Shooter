using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEntities.Ships.Guns.Inputs
{
    public class EnemyShipGunInput : IShipGunInput
    {
        public bool Shoot { get; private set; }

        public void UpdateInput() { }

        public void UpdateInput(bool shoot) => Shoot = shoot;
    }
}
