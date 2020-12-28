using GameEntities.Pools;
using Utilities.Configuration;

namespace GameEntities.Bullets
{
    public class PlayerHeavyLaserBullet : Bullet
    {
        public override void ReturnToPool() => PlayerHeavyLaserBulletPool.Instance.ReturnToPool(this);

        protected override (float bulletLifeDuration, float damage) GetBulletLifeAndDamage() => (
                ConfigurationUtils.HeavyLaserBulletConfig.BulletLifeDuration,
                ConfigurationUtils.HeavyLaserBulletConfig.BulletDamage
            );
    }
}
