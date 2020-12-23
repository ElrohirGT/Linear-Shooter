using GameEntities;

namespace GameEntities.Ships.Motors.Inputs
{
    public class AIMotorInput : IShipMotorInput
    {
        public float Rotation { get; private set; }

        public float Thrust { get; private set; }

        public void UpdateInput() { }

        public void UpdateInput(float thrust, float rotation)
        {
            Rotation = rotation;
            Thrust = thrust;
        }
    }
}