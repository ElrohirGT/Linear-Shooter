using GameEntities.Bullets;
using GameEntities.Pools;

namespace GameEntities.Ships.Guns
{
    public class PlayerLaserBulletGun : ShipGun<PlayerLaserBullet>
    {
        float _damageMultiplier = 1;

        public void SetDamageMultiplier(float damageMultiplier) => _damageMultiplier = damageMultiplier;

        protected override void ConfigureBulletAndStartBullet(PlayerLaserBullet bulletToShoot)
        {
            bulletToShoot.Damage *= _damageMultiplier;
            base.ConfigureBulletAndStartBullet(bulletToShoot);
        }

        protected override PlayerLaserBullet GetBulletFromPool() => PlayerLaserBulletPool.Instance.Get();
    }
}