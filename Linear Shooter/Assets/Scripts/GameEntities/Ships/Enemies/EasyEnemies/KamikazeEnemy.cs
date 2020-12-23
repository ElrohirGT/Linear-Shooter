using GameEntities.Pools;
using GameEntities.Ships.Motors;
using GameEntities.Ships.Motors.States;
using Utilities;
using Utilities.Configuration;

namespace GameEntities.Ships.Enemies.EasyEnemies
{
    /// <summary>
    /// An ememy that tries to hit the player in a certain period of time before exploding.
    /// </summary>
    public class KamikazeEnemy : ShipEnemy
    {
        Timer _lifeTimer;
        float _lifeDuration;

        protected override void Awake()
        {
            base.Awake();

            _lifeTimer = gameObject.AddComponent<Timer>();
            _lifeDuration = ConfigurationUtils.KamikazeEnemyConfig.LifeDuration;
            _lifeTimer.Finished += HandleAliveTimerFinished;
        }

        protected override float GetPointsWorth() => ConfigurationUtils.KamikazeEnemyConfig.PointsWorth;

        protected override StateMachine CreateShipMotorStateMachine() => new StateMachine(gameObject.AddComponent<PursuePlayerState>().Initialize(ShipMotorInput, ConfigurationUtils.KamikazeEnemyConfig.RotationCooldown));

        protected override ShipMotorSettings CreateMotorSettings()
        {
            return new ShipMotorSettings(
                ConfigurationUtils.KamikazeEnemyConfig.RotationSpeed,
                ConfigurationUtils.KamikazeEnemyConfig.ThrustAmount
            );
        }

        protected override (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues()
        {
            return (
                ConfigurationUtils.KamikazeEnemyConfig.InitialHitpoints,
                ConfigurationUtils.KamikazeEnemyConfig.InitialHitpoints,
                ConfigurationUtils.KamikazeEnemyConfig.BaseDamage,
                ConfigurationUtils.KamikazeEnemyConfig.DamageCooldownDuration
            );
        }

        void OnEnable() => _lifeTimer.StartTimer(_lifeDuration);

        void HandleAliveTimerFinished() => OnEntityTookDamage(float.MaxValue);

        public override void ResetEntity()
        {
            base.ResetEntity();
            _lifeTimer.ResetTimer();
        }

        public override void ReturnToPool() => KamikazeEnemiesPool.Instance.ReturnToPool(this);
    }
}
