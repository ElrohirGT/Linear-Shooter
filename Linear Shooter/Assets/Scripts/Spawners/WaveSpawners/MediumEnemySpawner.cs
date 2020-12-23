using Utilities;
using Utilities.Configuration;
using GameEntities.Pools;
using GameEntities.Ships.Enemies;
using System.Collections.Generic;

namespace Spawners.WaveSpawners
{
    public class MediumEnemySpawner : ShipEnemySpawner
    {
        protected override Dictionary<string, float> GetConfigurationTypesDictionary() => ConfigurationUtils.MediumEnemiesSpawnerConfig.ProbabilitiesByTypesNames;

        protected override WaveCheckPoint[] GetWaveGeneratorCheckpoints()
        {
            switch (GameManager.SelectedDifficulty)
            {
                case Difficulties.Easy:
                    return ConfigurationUtils.EasyWavesConfig.MediumEnemies.CheckPoints;
                case Difficulties.Medium:
                    return ConfigurationUtils.MediumWavesConfig.MediumEnemies.CheckPoints;
                case Difficulties.Hard:
                    return ConfigurationUtils.HardWavesConfig.MediumEnemies.CheckPoints;
                default:
                    return ConfigurationUtils.EasyWavesConfig.MediumEnemies.CheckPoints;
            }
        }

        protected override float GetWaveGeneratorGrowthValue()
        {
            switch (GameManager.SelectedDifficulty)
            {
                case Difficulties.Easy:
                    return ConfigurationUtils.EasyWavesConfig.MediumEnemies.GrowthValue;
                case Difficulties.Medium:
                    return ConfigurationUtils.MediumWavesConfig.MediumEnemies.GrowthValue;
                case Difficulties.Hard:
                    return ConfigurationUtils.HardWavesConfig.MediumEnemies.GrowthValue;
                default:
                    return ConfigurationUtils.EasyWavesConfig.MediumEnemies.GrowthValue;
            }
        }

        protected override float[] GetWaveGeneratorPattern()
        {
            switch (GameManager.SelectedDifficulty)
            {
                case Difficulties.Easy:
                    return ConfigurationUtils.EasyWavesConfig.MediumEnemies.Pattern;
                case Difficulties.Medium:
                    return ConfigurationUtils.MediumWavesConfig.MediumEnemies.Pattern;
                case Difficulties.Hard:
                    return ConfigurationUtils.HardWavesConfig.MediumEnemies.Pattern;
                default:
                    return ConfigurationUtils.EasyWavesConfig.MediumEnemies.Pattern;
            }
        }
    }
}
