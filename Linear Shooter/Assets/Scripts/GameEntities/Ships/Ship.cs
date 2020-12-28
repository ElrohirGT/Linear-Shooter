using GameEntities.Ships.Motors;
using GameEntities.Ships.Motors.Inputs;
using Utilities;

namespace GameEntities.Ships
{
    /// <summary>
    /// Base class for all ships in the game. This class will never be directly instantiated.
    /// </summary>
    public abstract class Ship : AliveEntity
    {
        /// <summary>
        /// Responds to the corresponding movement input's.
        /// </summary>
        ShipMotor _shipMotor;

        /// <summary>
        /// Determines the input's that this entity takes, so it can respond to it.
        /// </summary>
        IShipMotorInput _shipMotorInput;

        /// <summary>
        /// Get's the settings that modify the motor of the ship.
        /// </summary>
        ShipMotorSettings _shipMotorSettings;

        /// <summary>
        /// Manages all the states of the ship motor.
        /// </summary>
        StateMachine _shipMotorStateMachine;

        /// <summary>
        /// Get's the ship's motor's state machine.
        /// </summary>
        protected StateMachine ShipMotorStateMachine => _shipMotorStateMachine;

        protected ShipMotorSettings ShipMotorSettings => _shipMotorSettings;

        /// <summary>
        /// Get's this ship's motor's input.
        /// </summary>
        protected IShipMotorInput ShipMotorInput => _shipMotorInput;

        /// <summary>
        /// Initializes the ship.
        /// If this ship doesn't need to do anything extra to initialize itself it shouldn't override this method.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            _shipMotorInput = CreateMotorInput();
            _shipMotorSettings = CreateMotorSettings();
            _shipMotorStateMachine = CreateShipMotorStateMachine();

            _shipMotor = new ShipMotor(_shipMotorInput, transform, _shipMotorSettings, _shipMotorStateMachine);
        }
        /// <summary>
        /// Updates the input and ticks the motor.
        /// </summary>
        protected virtual void FixedUpdate() => _shipMotor.Tick();

        /// <summary>
        /// Creates the state machine the motor will use.
        /// </summary>
        /// <returns>The state machine the motor will use.</returns>
        protected abstract StateMachine CreateShipMotorStateMachine();

        /// <summary>
        /// Creates the motor input that this ship will use.
        /// </summary>
        /// <returns>The motor this ship will use.</returns>
        protected abstract IShipMotorInput CreateMotorInput();

        /// <summary>
        /// Creates the motor settings the motor this ship will use.
        /// </summary>
        /// <returns>The motor settings this ship will use.</returns>
        protected abstract ShipMotorSettings CreateMotorSettings();
    }
}