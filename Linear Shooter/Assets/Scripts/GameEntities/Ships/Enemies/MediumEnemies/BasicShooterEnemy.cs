using GameEntities.Pools;
using GameEntities.Ships.Guns;
using GameEntities.Ships.Guns.Inputs;
using GameEntities.Ships.Guns.States;
using GameEntities.Ships.Motors;
using GameEntities.Ships.Motors.States;
using UnityEngine;
using Utilities;
using Utilities.Configuration;

namespace GameEntities.Ships.Enemies.MediumEnemies
{
    public class BasicShooterEnemy : ShipEnemy
    {
        readonly IShipGunInput _shipGunInput = new EnemyShipGunInput();
        EnemyLaserBulletGun _shipGun;
        ShipGunSettings _shipGunSettings;
        StateMachine _shipGunStateMachine;

        protected override void Awake()
        {
            base.Awake();

            IState normalGunState = new FirePlayerState(_shipGunInput);
            IState freezerEffectState = new FreezeGunState(_shipGunInput);

            _shipGunStateMachine = new StateMachine(normalGunState);
            _shipGunStateMachine.AddAnyTransition(freezerEffectState, () => EffectManager.Instance.IsFreezerEffectActivated);
            _shipGunStateMachine.AddTransition(freezerEffectState, normalGunState, () => !EffectManager.Instance.IsFreezerEffectActivated);

            _shipGun = gameObject.AddComponent<EnemyLaserBulletGun>();

            foreach (var item in ConfigurationUtils.BasicShooterEnemyConfig.Guns)
            {
                if (item.Key.Equals(_shipGun.BulletTypeName))
                {
                    _shipGunSettings = new ShipGunSettings(
                        item.Value.ShootCooldownDuration,
                        item.Value.BulletInitialImpulseMagnitude
                    );
                }
            }

            _shipGun.Initialize(_shipGunInput, _shipGunSettings, _shipGunStateMachine);
        }

        protected override float GetPointsWorth() => ConfigurationUtils.BasicShooterEnemyConfig.PointsWorth;

        protected override StateMachine CreateShipMotorStateMachine()
        {
            PursuePlayerState pursuePlayerState = gameObject.AddComponent<PursuePlayerState>().Initialize(ShipMotorInput, ConfigurationUtils.BasicShooterEnemyConfig.RotationCooldown);
            FleeFromPlayerState fleeFromPlayerState = gameObject.AddComponent<FleeFromPlayerState>().Initialize(ShipMotorInput, ConfigurationUtils.BasicShooterEnemyConfig.RotationCooldown);

            var stateMachine = new StateMachine(pursuePlayerState);

            stateMachine.AddTransition(pursuePlayerState, fleeFromPlayerState, () => Vector3.Distance(Player.Position, transform.position) <= ScreenUtils.WorldWidth / 3);
            stateMachine.AddTransition(fleeFromPlayerState, pursuePlayerState, () => Vector3.Distance(Player.Position, transform.position) >= ScreenUtils.WorldWidth / 2);

            return stateMachine;
        }

        protected override ShipMotorSettings CreateMotorSettings()
        {
            return new ShipMotorSettings(
                ConfigurationUtils.BasicShooterEnemyConfig.RotationSpeed,
                ConfigurationUtils.BasicShooterEnemyConfig.ThrustAmount
            );
        }

        protected override (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues()
        {
            return (
                ConfigurationUtils.BasicShooterEnemyConfig.InitialHitpoints,
                ConfigurationUtils.BasicShooterEnemyConfig.InitialHitpoints,
                ConfigurationUtils.BasicShooterEnemyConfig.BaseDamage,
                ConfigurationUtils.BasicShooterEnemyConfig.DamageCooldownDuration
            );
        }

        public override void ReturnToPool() => BasicShooterEnemiesPool.Instance.ReturnToPool(this);
    }
}
