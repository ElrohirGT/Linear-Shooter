using GameEntities;

namespace GameEntities.Ships.Motors.Inputs
{
    public interface IShipMotorInput : IShipInput
    {
        /// <summary>
        /// Represents the input to rotate the ship.
        /// A values between -1, 1 that tells the motor how much to rotate.
        /// </summary>
        float Rotation { get; }
        /// <summary>
        /// Represents the input to thrust the ship.
        /// A value between -1, 1 that tells the motor how much to move.
        /// </summary>
        float Thrust { get; }

        /// <summary>
        /// Updates the input with the given values.
        /// This method exits so that AI can input their custom inputs.
        /// </summary>
        /// <param name="thrust">The thrust of the ship, a value between -1,1.</param>
        /// <param name="rotation">The rotation of the ship, a value between -1,1.</param>
        void UpdateInput(float thrust, float rotation);
    }
}