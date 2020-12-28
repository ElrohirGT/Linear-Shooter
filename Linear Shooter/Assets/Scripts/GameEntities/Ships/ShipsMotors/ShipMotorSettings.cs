namespace GameEntities.Ships.Motors
{
    public class ShipMotorSettings
    {
        /// <summary>
        /// Get's the rotation speed of this ship.
        /// </summary>
        public float RotationSpeed { get; private set; }
        /// <summary>
        /// Get's the thrust amount of this ship.
        /// </summary>
        public float ThrustAmount { get; private set; }

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

        public void ScaleSettings(float rotationSpeedScaleFactor, float thrustAmountScaleFactor)
        {
            RotationSpeed *= rotationSpeedScaleFactor;
            ThrustAmount *= thrustAmountScaleFactor;
        }
    }
}