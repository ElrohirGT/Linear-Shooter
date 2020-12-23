using System;
using GameEntities.Pools;
using GameEntities.Ships.PlayerShips;
using UnityEngine;
using Utilities.Configuration;
using Utilities.Constants;

namespace GameEntities.PowerUps
{
    public class ExtraLifePowerUp : PowerUp
    {
        /*TODO: to make the extra life powerup localized to the player that picks up the power up
         * we just make the event require an argument that gives
         * the playership instance that activated the power up
         * like this:
         * public static Action<PlayerShip> PickedUp;*/
        public static Action PickedUp;

        protected override float GetLifeDuration() => ConfigurationUtils.ExtraLifePowerUpConfig.LifeDuration;

        public override void ReturnToPool() => ExtraLifePowerUpPool.Instance.ReturnToPool(this);

        protected override void OnPlayerPickedUp() => PickedUp?.Invoke();
    }
}
