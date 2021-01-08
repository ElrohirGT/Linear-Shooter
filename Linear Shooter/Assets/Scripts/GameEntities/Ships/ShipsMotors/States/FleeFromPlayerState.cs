using System;
using GameEntities.Ships.Motors.Inputs;
using UnityEngine;
using Utilities;

namespace GameEntities.Ships.Motors.States
{
    /// <summary>
    /// This class is teh same as PursuePlayerState except the input is inverted
    /// so it aims to the opposite direction the player is, instead of the usual.
    /// </summary>
    public class FleeFromPlayerState : MonoBehaviour, IShipMotorState
    {
        IShipMotorInput _shipMotorInput;
        Timer _rotationCooldownTimer;
        float _rotationCooldownDuration;

        float _currenRotationInput = 0;

        public IShipMotorInput ShipMotorInput => _shipMotorInput;
        public Timer RotationCooldownTimer => _rotationCooldownTimer;
        public float RotationCooldownDuration => _rotationCooldownDuration;

        public event Action Entered;
        public event Action Exited;

        /// <summary>
        /// Initializes the state and all it needs to function.
        /// </summary>
        /// <param name="shipMotorInput">The input manager of the motor.</param>
        /// <param name="rotationCooldownDuration">The cooldown duration for rotating the ship.</param>
        /// <returns>This instance of the state.</returns>
        public FleeFromPlayerState Initialize(IShipMotorInput shipMotorInput, float rotationCooldownDuration)
        {
            _shipMotorInput = shipMotorInput;
            _rotationCooldownDuration = rotationCooldownDuration;
            return this;
        }

        void Awake() => _rotationCooldownTimer = gameObject.AddComponent<Timer>();

        public void OnEnter()
        {
            _rotationCooldownTimer.Finished += HandleRotationCooldownTimerFinished;
            UpdateRotation();

            Entered?.Invoke();
        }

        public void OnExit()
        {
            _rotationCooldownTimer.Finished -= HandleRotationCooldownTimerFinished;
            _rotationCooldownTimer.ResetTimer();

            Exited?.Invoke();
        }

        public void Tick()
        {
            //1 is the max thrust and rotation expected.
            _shipMotorInput.UpdateInput(
                Mathf.Clamp(Vector3.Distance(transform.position, GameManager.PlayerPosition), 0.2f, 1.5f),
                _currenRotationInput);
        }

        void HandleRotationCooldownTimerFinished() => UpdateRotation();

        void UpdateRotation()
        {
            _currenRotationInput = -CustomMethods.CalculateRotationInput(transform, GameManager.PlayerPosition, 0.4f);

            Tick();
            _rotationCooldownTimer.StartTimer(_rotationCooldownDuration);
        }
    }
}