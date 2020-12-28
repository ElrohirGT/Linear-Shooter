using System;
using GameEntities.Bullets;
using GameEntities.Pools;

namespace GameEntities.Ships.Guns
{
    public class PlayerHeavyLaserBulletGun : ShipGun<PlayerHeavyLaserBullet>, IShipGun<PlayerHeavyLaserBullet>
    {
        protected override PlayerHeavyLaserBullet GetBulletFromPool() => PlayerHeavyLaserBulletPool.Instance.Get();
    }
}
