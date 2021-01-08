using System;
using Events;
using GameEntities.Pools;
using UnityEngine;
using Utilities.Configuration;

namespace GameEntities.PowerUps
{
    public class FreezerPowerUp : PowerUp
    {
        public static Action<FreezerPowerUpPickedUpEventInfo> PickedUp;

        public override void ReturnToPool() => FreezerPowerUpPool.Instance.ReturnToPool(this);

        protected override float GetLifeDuration() => ConfigurationUtils.FreezerPowerUpConfig.LifeDuration;

        protected override void OnPlayerPickedUp() => PickedUp?.Invoke(new FreezerPowerUpPickedUpEventInfo(ConfigurationUtils.FreezerPowerUpConfig.EffectDuration));
    }
}
