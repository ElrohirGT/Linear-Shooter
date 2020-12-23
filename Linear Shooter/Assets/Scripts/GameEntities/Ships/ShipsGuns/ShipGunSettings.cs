namespace GameEntities.Ships.Guns
{
    public class ShipGunSettings
    {
        /// <summary>
        /// Get's the time the gun waits before shooting again.
        /// </summary>
        public float ShootCooldownDuration { get; set; }

        /// <summary>
        /// Get's the initial impulse the gun gives to it's bullet before firing.
        /// </summary>
        public float BulletInitialImpulseMagnitud { get; set; }

        public ShipGunSettings(float shootCooldownDuration, float bulletInitialImpulseMagnitud)
        {
            ShootCooldownDuration = shootCooldownDuration;
            BulletInitialImpulseMagnitud = bulletInitialImpulseMagnitud;
        }
    }
}