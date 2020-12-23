using System.Collections.Generic;
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;

namespace Utilities.Configuration
{
    public partial class EnemiesConfig
    {
        [JsonConstructor]
        public EnemiesConfig(EasyEnemiesConfig easyEnemies, MediumEnemiesConfig mediumEnemies)
        {
            EasyEnemies = easyEnemies;
            MediumEnemies = mediumEnemies;
        }

        [J("EasyEnemies", Required = R.Always)] public EasyEnemiesConfig EasyEnemies { get; }
        [J("MediumEnemies", Required = R.Always)] public MediumEnemiesConfig MediumEnemies { get; }
    }

    public partial class EasyEnemiesConfig
    {
        [JsonConstructor]
        public EasyEnemiesConfig(EnemyShipConfig basicEnemy, KamikazeEnemyConfig kamikazeEnemy)
        {
            BasicEnemy = basicEnemy;
            KamikazeEnemy = kamikazeEnemy;
        }

        [J("BasicEnemy", Required = R.Always)] public EnemyShipConfig BasicEnemy { get; }
        [J("KamikazeEnemy", Required = R.Always)] public KamikazeEnemyConfig KamikazeEnemy { get; }
    }

    public partial class MediumEnemiesConfig
    {
        [JsonConstructor]
        public MediumEnemiesConfig(EnemyShipConfig basicShooterEnemy)
        {
            BasicShooterEnemy = basicShooterEnemy;
        }

        [J("BasicShooterEnemy", Required = R.Always)] public EnemyShipConfig BasicShooterEnemy { get; }
    }

    public class EnemyShipConfig : ShipConfig
    {
        [JsonConstructor]
        public EnemyShipConfig(float pointsWorth, float rotationSpeed, float rotationCooldown, float thrustAmount, Dictionary<string, GunConfig> guns, float maxHitpoints, float initialHitpoints, float baseDamage, float damageCooldownDuration)
            : base(rotationSpeed, thrustAmount, guns, maxHitpoints, initialHitpoints, baseDamage, damageCooldownDuration)
        {
            PointsWorth = pointsWorth;
            RotationCooldown = rotationCooldown;
        }

        [J("RotationCooldown", Required = R.Always)] public float RotationCooldown { get; }
        [J("PointsWorth", Required = R.Always)] public float PointsWorth { get; }
    }
    public class KamikazeEnemyConfig : EnemyShipConfig
    {
        [JsonConstructor]
        public KamikazeEnemyConfig(float lifeDuration, float pointsWorth, float rotationSpeed, float rotationCooldown, float thrustAmount, Dictionary<string, GunConfig> guns, float maxHitpoints, float initialHitpoints, float baseDamage, float damageCooldownDuration)
            : base(pointsWorth, rotationSpeed, rotationCooldown, thrustAmount, guns, maxHitpoints, initialHitpoints, baseDamage, damageCooldownDuration)
        {
            LifeDuration = lifeDuration;
        }

        [J("LifeDuration", Required = R.Always)] public float LifeDuration { get; }
    }
}