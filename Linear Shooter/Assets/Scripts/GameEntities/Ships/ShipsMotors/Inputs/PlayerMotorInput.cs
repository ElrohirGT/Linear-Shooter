using UnityEngine;
using Utilities.Constants;

namespace GameEntities.Ships.Motors.Inputs
{
    public class PlayerMotorInput : IShipMotorInput
    {
        public float Rotation { get; private set; }

        public float Thrust { get; private set; }

        public void UpdateInput()
        {
            Rotation = Input.GetAxis(InputAxisConstants.HORIZONTAL);
            Thrust = Input.GetAxis(InputAxisConstants.VERTICAL);
        }

        public void UpdateInput(float thrust, float rotation) { }

        public override string ToString() => $"Rotation: {Rotation} | Thrust: {Thrust}";
    }
}