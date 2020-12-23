using Utilities.Configuration;
using GameEntities.Pools;

namespace GameEntities.Bullets
{
    public class PlayerLaserBullet : Bullet
    {
        protected override (float bulletLifeDuration, float damage) GetBulletLifeAndDamage()
        {
            return (
                ConfigurationUtils.LaserBulletConfig.BulletLifeDuration,
                ConfigurationUtils.LaserBulletConfig.BulletDamage
            );
        }

        public override void ReturnToPool() => PlayerLaserBulletPool.Instance.ReturnToPool(this);
    }
}
