using System;
using UnityEngine;
using Utilities.Configuration;

namespace GameEntities.Shields
{
    public class ShieldEnemyShield : Shield
    {
        protected override void Awake()
        {
            base.Awake();
            Vector3 scale = new Vector3(ConfigurationUtils.ShieldEnemy.ShieldScale, ConfigurationUtils.ShieldEnemy.ShieldScale, 1);
            transform.localScale = scale;
        }
        protected override (float maxHitpoints, float currentHitpoints, float baseDamage, float damageCooldownDuration) GetInitializationValues() => (
                ConfigurationUtils.ShieldEnemy.ShieldLife,
                ConfigurationUtils.ShieldEnemy.ShieldLife,
                0,
                ConfigurationUtils.ShieldEnemy.DamageCooldownDuration
            );
    }
}