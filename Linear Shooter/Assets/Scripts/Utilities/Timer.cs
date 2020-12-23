using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utilities
{
    public class Timer : MonoBehaviour
    {

        /*
        .######..######..######..##......#####....####..
        .##........##....##......##......##..##..##.....
        .####......##....####....##......##..##...####..
        .##........##....##......##......##..##......##.
        .##......######..######..######..#####....####..
        ................................................
        */
        /// <summary>
        /// Get's the duration the timer counts.
        /// </summary>
        private float _duration = 0;

        /// <summary>
        /// Get's the seconds that have passed since the timer started.
        /// </summary>
        private float _elapsedSeconds = 0;

        /*
        .#####...#####....####...#####...######..#####...######..######..######...####..
        .##..##..##..##..##..##..##..##..##......##..##....##......##....##......##.....
        .#####...#####...##..##..#####...####....#####.....##......##....####.....####..
        .##......##..##..##..##..##......##......##..##....##......##....##..........##.
        .##......##..##...####...##......######..##..##....##....######..######...####..
        ................................................................................
        */
        /// <summary>
        /// Get's wether the timer is currently running or not.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        public float Duration => _duration;

        /// <summary>
        /// Get's how many seconds are left on the timer, if the timer is not running returns 0.
        /// </summary>
        public float RemainingSeconds => _duration - _elapsedSeconds;

        public float ElapsedSeconds => _elapsedSeconds;

        /// <summary>
        /// An event that fires off once the timer has finished counting.
        /// </summary>
        public event Action Finished;

        /*
         .##...##..######..######..##..##...####...#####....####..
         .###.###..##........##....##..##..##..##..##..##..##.....
         .##.#.##..####......##....######..##..##..##..##...####..
         .##...##..##........##....##..##..##..##..##..##......##.
         .##...##..######....##....##..##...####...#####....####..
         .........................................................
        */
        #region Unity

        void Update()
        {
            if (!IsRunning)
                return;

            _elapsedSeconds += Time.deltaTime;

            if (_elapsedSeconds < _duration)
                return;

            OnFinished();
        }

        #endregion

        #region Privates
        /// <summary>
        /// Invokes the Finished event, if it's a cycle then the timer starts again.
        /// </summary>
        private void OnFinished()
        {
            IsRunning = false;
            Finished?.Invoke();
        }
        #endregion

        #region API

        /// <summary>
        /// Starts the timer with the given duration, only if duration is greater than 0.
        /// </summary>
        /// <param name="duration">The duration the timer will run.</param>
        public void StartTimer(float duration)
        {
            if (duration <= 0)
                throw new ArgumentException("Duration must be greater than 0.", nameof(duration));

            _duration = duration;
            _elapsedSeconds = 0;
            IsRunning = true;
        }

        /// <summary>
        /// Stops and resets the timer.
        /// Calling this method reset's the timer to a state were you never called </c>StartTimer</c>.
        /// </summary>
        public void ResetTimer()
        {
            IsRunning = false;
            _duration = 0;
            _elapsedSeconds = 0;
        }

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        public void Pause() => IsRunning = false;

        /// <summary>
        /// Unpauses the timer.
        /// </summary>
        public void UnPause() => IsRunning = true;
        #endregion
    }
}