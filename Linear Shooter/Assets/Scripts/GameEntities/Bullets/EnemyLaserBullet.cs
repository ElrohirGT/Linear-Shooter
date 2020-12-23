using Utilities.Configuration;
using GameEntities.Pools;

namespace GameEntities.Bullets
{
    public class EnemyLaserBullet : Bullet
    {
        protected override (float bulletLifeDuration, float damage) GetBulletLifeAndDamage()
        {
            return (
                ConfigurationUtils.LaserBulletConfig.BulletLifeDuration,
                ConfigurationUtils.LaserBulletConfig.BulletDamage
            );
        }

        public override void ReturnToPool() => EnemyLaserBulletPool.Instance.ReturnToPool(this);
    }
}
