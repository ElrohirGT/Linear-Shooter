using System.Collections.Generic;
using Utilities.Configuration;

namespace Spawners.WaveSpawners
{
    public class WaveGenerator
    {
        /// <summary>
        /// Determines by how much (in a percentage) the entities will grow.
        /// </summary>
        readonly float _growthValue;
        /// <summary>
        /// The pattern in which the entities will spawn.
        /// </summary>
        readonly float[] _pattern;
        /// <summary>
        /// The current index of in the pattern.
        /// </summary>
        int _currentWaveIndex = -1;

        /// <summary>
        /// Get's the checkpoints of this wave generator.
        /// </summary>
        readonly WaveCheckPoint[] _checkpoints;

        /// <summary>
        /// Get's the current number of the wave.
        /// </summary>
        int _currentWaveNumber = 0;

        public WaveGenerator(float growthValue, float[] pattern, WaveCheckPoint[] checkPoints)
        {
            _growthValue = growthValue;
            _pattern = pattern;
            _checkpoints = checkPoints;
        }

        /// <summary>
        /// Gets the next wave to spawn.
        /// </summary>
        /// <returns>The next wave to spawn.</returns>
        public int GetWave()
        {
            _currentWaveNumber++;

            if (ValidCheckpoint(out int checkPointQuantityToSpawn))
                return checkPointQuantityToSpawn;

            return NextWave();
        }

        /// <summary>
        /// Checks if there is a checkpoint for the current wave.
        /// </summary>
        /// <param name="checkPointValue">The quantity the checkpoint will spawn.</param>
        /// <returns>True if a checkpoint was found, false otherwise.</returns>
        bool ValidCheckpoint(out int checkPointValue)
        {
            checkPointValue = 0;

            for (int i = _checkpoints.Length - 1; i >= 0; i--)
            {
                if (_checkpoints[i].WaveNumber == _currentWaveNumber)
                {
                    checkPointValue = _checkpoints[i].QuantityToSpawn;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get's the next wave from the pattern.
        /// </summary>
        /// <returns>The quantity of entities to spawn.</returns>
        int NextWave()
        {
            _currentWaveIndex++;

            if (_currentWaveIndex == _pattern.Length)
            {
                GrowPattern();
                _currentWaveIndex = 0;
            }

            return (int)_pattern[_currentWaveIndex];
        }

        /// <summary>
        /// Grows the pattern by the supplied amount.
        /// </summary>
        void GrowPattern()
        {
            for (int i = 0; i < _pattern.Length; i++)
                _pattern[i] *= 1 + _growthValue;
        }
    }
}
