namespace GameEntities.Ships.Motors
{
    public struct ShipMotorSettings
    {
        /// <summary>
        /// Get's the rotation speed of this ship.
        /// </summary>
        public float RotationSpeed { get; }
        /// <summary>
        /// Get's the thrust amount of this ship.
        /// </summary>
        public float ThrustAmount { get; }

        /// <summary>
        /// It's in charge of the configuration of a ship,
        /// it's movement speed and rotation speed are set here.
        /// </summary>
        /// <param name="rotationSpeed">The rotation speed of this ship.</param>
        /// <param name="thrustAmount">The thrust amount of this ship.</param>
        public ShipMotorSettings(float rotationSpeed, float thrustAmount)
        {
            RotationSpeed = rotationSpeed;
            ThrustAmount = thrustAmount;
        }
    }
}