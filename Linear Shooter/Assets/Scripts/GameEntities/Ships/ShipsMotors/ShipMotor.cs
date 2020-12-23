using UnityEngine;
using GameEntities.Ships.Motors.Inputs;
using Utilities;

namespace GameEntities.Ships.Motors
{
    public class ShipMotor
    {
        /// <summary>
        /// Manages the states of this ship's motor.
        /// </summary>
        readonly StateMachine _stateMachine;

        /// <summary>
        /// Gives the input from users.
        /// </summary>
        readonly IShipMotorInput _shipEntityInput;

        /// <summary>
        /// The transform of a gameobject to move.
        /// </summary>
        readonly Transform _transformToMove;

        /// <summary>
        /// The settings this motor works on.
        /// </summary>
        readonly ShipMotorSettings _shipMotorSettings;

        /// <summary>
        /// Represents a ship motor, it's in charge of moving the ship according to some input.
        /// </summary>
        /// <param name="shipEntityInput">The input system for this motor.</param>
        /// <param name="transformToMove">The transform of the ship to move.</param>
        /// <param name="shipSettings">The settings that modify this motor.</param>
        public ShipMotor(IShipMotorInput shipEntityInput, Transform transformToMove, ShipMotorSettings shipMotorSettings, StateMachine stateMachine)
        {
            _shipEntityInput = shipEntityInput;
            _transformToMove = transformToMove;
            _shipMotorSettings = shipMotorSettings;
            _stateMachine = stateMachine;
        }

        /// <summary>
        /// Moves the transform according to the input.
        /// </summary>
        public void Tick()
        {
            _stateMachine.Tick();

            //The -1 inverts the axis such that right arrow will turn the ship in a clockwise direction.
            _transformToMove.Rotate(_transformToMove.forward, -1 * _shipEntityInput.Rotation * Time.fixedDeltaTime * _shipMotorSettings.RotationSpeed);
            _transformToMove.position += _transformToMove.up * _shipEntityInput.Thrust * Time.fixedDeltaTime * _shipMotorSettings.ThrustAmount;
        }
    }
}