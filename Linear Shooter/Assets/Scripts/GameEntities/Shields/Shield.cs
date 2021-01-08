using System;
using GameEntities.Bullets;
using Utilities.Constants;

namespace GameEntities.Shields
{
    public abstract class Shield : AliveEntity
    {
        void OnDisable() => ResetShield();
        public virtual void ResetShield() => InitializeEntity();

        void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            if (!collision.CompareTag(TagsConstants.PLAYER_BULLET))
                return;

            Bullet bullet = collision.GetComponent<Bullet>();
            OnEntityTookDamage(bullet.Damage);
            bullet.ReturnToPool();
        }
    }
}
