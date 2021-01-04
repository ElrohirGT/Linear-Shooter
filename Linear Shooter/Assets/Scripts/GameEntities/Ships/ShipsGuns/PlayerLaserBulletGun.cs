using GameEntities.Bullets;
using GameEntities.Pools;
using UnityEngine;

namespace GameEntities.Ships.Guns
{
    public class PlayerLaserBulletGun : PlayerShipGun<PlayerLaserBullet>
    {
        protected override PlayerLaserBullet GetBulletFromPool() => PlayerLaserBulletPool.Instance.Get();
    }
}