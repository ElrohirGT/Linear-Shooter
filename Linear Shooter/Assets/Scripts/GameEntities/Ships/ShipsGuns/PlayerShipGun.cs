using System;
using GameEntities.Bullets;
using UnityEngine;
using Utilities;

namespace GameEntities.Ships.Guns
{
    public abstract class PlayerShipGun<T> : ShipGun<T> where T : Bullet
    {
        float _damageMultiplier = 1;

        public void SetDamageMultiplier(float damageMultiplier) => _damageMultiplier = damageMultiplier;

        protected override void ConfigureBulletAndStartBullet(T bulletToShoot)
        {
            Vector3 mouseWorldPosition = ScreenUtils.MainCamera.ScreenToWorldPoint(Input.mousePosition);

            bulletToShoot.Damage *= _damageMultiplier;
            bulletToShoot.transform.position = transform.position;
            CustomMethods.LookAt2D(bulletToShoot.transform, mouseWorldPosition);

            bulletToShoot.Initialize(ShipGunSettings.BulletInitialImpulseMagnitud, false);
        }
    }
}
