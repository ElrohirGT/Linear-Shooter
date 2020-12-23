using GameEntities.Pools;
using GameEntities.Ships.Motors;
using GameEntities.Ships.Motors.States;
using Utilities;
using Utilities.Configuration;

namespace GameEntities.Ships.Enemies.EasyEnemies
{
    public class BasicEnemy : ShipEnemy
    {
        protected override StateMachine CreateShipMotorStateMachine() => new StateMachine(gameObject.AddComponent<PursuePlayerState>().Initialize(ShipMotorInput, ConfigurationUtils.BasicEnemyConfig.RotationCooldown));

        protected override ShipMotorSettings CreateMotorSettings()
        {
            return new ShipMotorSettings(
                ConfigurationUtils.BasicEnemyConfig.RotationSpeed,
                ConfigurationUtils.BasicEnemyConfig.ThrustAmount
            );
        }

        protected override (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues()
        {
            return (
                ConfigurationUtils.BasicEnemyConfig.InitialHitpoints,
                ConfigurationUtils.BasicEnemyConfig.InitialHitpoints,
                ConfigurationUtils.BasicEnemyConfig.BaseDamage,
                ConfigurationUtils.BasicEnemyConfig.DamageCooldownDuration
            );
        }

        protected override float GetPointsWorth() => ConfigurationUtils.BasicEnemyConfig.PointsWorth;

        public override void ReturnToPool() => BasicEnemiesPool.Instance.ReturnToPool(this);
    }
}
