using Utilities;
using Utilities.Configuration;
using GameEntities.Pools;
using GameEntities.Ships.Enemies;
using System.Collections.Generic;

namespace Spawners.WaveSpawners
{
    /// <summary>
    /// Spawns enemies in the game.
    /// </summary>
    public class EasyEnemySpawner : ShipEnemySpawner
    {
        protected override Dictionary<string, float> GetConfigurationTypesDictionary() => ConfigurationUtils.EasyEnemiesSpawnerConfig.ProbabilitiesByTypesNames;

        protected override float[] GetWaveGeneratorPattern()
        {
            switch (GameManager.SelectedDifficulty)
            {
                case Difficulties.Easy:
                    return ConfigurationUtils.EasyWavesConfig.EasyEnemies.Pattern;
                case Difficulties.Medium:
                    return ConfigurationUtils.MediumWavesConfig.EasyEnemies.Pattern;
                case Difficulties.Hard:
                    return ConfigurationUtils.HardWavesConfig.EasyEnemies.Pattern;
                default:
                    return ConfigurationUtils.EasyWavesConfig.EasyEnemies.Pattern;
            }
        }

        protected override float GetWaveGeneratorGrowthValue()
        {
            switch (GameManager.SelectedDifficulty)
            {
                case Difficulties.Easy:
                    return ConfigurationUtils.EasyWavesConfig.EasyEnemies.GrowthValue;
                case Difficulties.Medium:
                    return ConfigurationUtils.MediumWavesConfig.EasyEnemies.GrowthValue;
                case Difficulties.Hard:
                    return ConfigurationUtils.HardWavesConfig.EasyEnemies.GrowthValue;
                default:
                    return ConfigurationUtils.EasyWavesConfig.EasyEnemies.GrowthValue;
            }
        }

        protected override WaveCheckPoint[] GetWaveGeneratorCheckpoints()
        {
            switch (GameManager.SelectedDifficulty)
            {
                case Difficulties.Easy:
                    return ConfigurationUtils.EasyWavesConfig.EasyEnemies.CheckPoints;
                case Difficulties.Medium:
                    return ConfigurationUtils.MediumWavesConfig.EasyEnemies.CheckPoints;
                case Difficulties.Hard:
                    return ConfigurationUtils.HardWavesConfig.EasyEnemies.CheckPoints;
                default:
                    return ConfigurationUtils.EasyWavesConfig.EasyEnemies.CheckPoints;
            }
        }
    }
}