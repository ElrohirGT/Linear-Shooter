using System;
using GameEntities.Bullets;
using GameEntities.Pools;

namespace GameEntities.Ships.Guns
{
    public class EnemyLaserBulletGun : ShipGun<EnemyLaserBullet>
    {
        protected override EnemyLaserBullet GetBulletFromPool() => EnemyLaserBulletPool.Instance.Get();
    }
}
