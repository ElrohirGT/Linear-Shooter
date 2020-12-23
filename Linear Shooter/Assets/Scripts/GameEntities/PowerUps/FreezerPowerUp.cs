using System;
using Events;
using GameEntities.Pools;
using UnityEngine;
using Utilities.Configuration;

namespace GameEntities.PowerUps
{
    public class FreezerPowerUp : PowerUp
    {
        /*TODO: to make the extra life powerup localized to the player that picks up the power up
         * we just make the event require an argument that gives
         * the playership instance that activated the power up
         * like this:
         * public static Action<PlayerShip> PickedUp;*/
        public static Action<FreezerPowerUpPickedUpEventInfo> PickedUp;

        public override void ReturnToPool() => FreezerPowerUpPool.Instance.ReturnToPool(this);

        protected override float GetLifeDuration() => ConfigurationUtils.FreezerPowerUpConfig.LifeDuration;

        protected override void OnPlayerPickedUp() => PickedUp?.Invoke(new FreezerPowerUpPickedUpEventInfo(ConfigurationUtils.FreezerPowerUpConfig.EffectDuration));
    }
}
