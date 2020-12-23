using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Utilities.Configuration
{
    /// <summary>
    /// Provides configuration parameters to the whole application.
    /// </summary>
    public static class ConfigurationUtils
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
        /// Get's the file name of the .json that holds all the custom values.
        /// </summary>
        const string _FILE_NAME = "Configurations.json";

        //TODO Complete default configurations.
        /// <summary>
        /// The default configurations that will be used if reading the file fails.
        /// </summary>
        static readonly ConfigurationValues _DEFAULT_CONFIGURATION_VALUES;

        /// <summary>
        /// Get's wether <c>CongifurationUtils</c> has or hasn't been initialized.
        /// </summary>
        static bool _alreadyInitialized = false;

        /// <summary>
        /// An object that contains all the custom values of the game.
        /// </summary>
        static ConfigurationValues _configurationValues;

        /* 
        .#####...#####....####...#####...######..#####...######..######..######...####..
        .##..##..##..##..##..##..##..##..##......##..##....##......##....##......##.....
        .#####...#####...##..##..#####...####....#####.....##......##....####.....####..
        .##......##..##..##..##..##......##......##..##....##......##....##..........##.
        .##......##..##...####...##......######..##..##....##....######..######...####..
        ................................................................................
        */
        #region Properties

        #region MedalsConfig
        public static MedalsConfig MedalsConfig => _configurationValues.Medals;
        #endregion

        #region Waves Config
        /// <summary>
        /// Get's the waves configuration for the easy difficutly.
        /// </summary>
        public static DifficultyWavesConfig EasyWavesConfig => _configurationValues.Waves.EasyWaves;

        /// <summary>
        /// Get's the waves configuration for the medium difficutly.
        /// </summary>
        public static DifficultyWavesConfig MediumWavesConfig => _configurationValues.Waves.MediumWaves;

        /// <summary>
        /// Get's the waves configuration for the hard difficutly.
        /// </summary>
        public static DifficultyWavesConfig HardWavesConfig => _configurationValues.Waves.HardWaves;
        #endregion

        #region Spawners Config
        public static ProbabilitySpawnerConfig PowerUpSpawnerConfig => _configurationValues.Spawners.PowerUpSpawner;

        public static TimedSpawnerConfig MedalSpawnerConfig => _configurationValues.Spawners.MedalSpawner;

        public static WaveSpawnerConfig EasyEnemiesSpawnerConfig => _configurationValues.Spawners.EasyEnemiesSpawner;

        public static WaveSpawnerConfig MediumEnemiesSpawnerConfig => _configurationValues.Spawners.MediumEnemiesSpawner;
        #endregion

        #region PlayerShips Config
        /// <summary>
        /// Get's the ships the player controls configuration.
        /// </summary>
        public static PlayerShipsConfig PlayerShipsConfig => _configurationValues.GameEntities.PlayerShips;
        #endregion

        #region PowerUps Config
        public static PowerUpConfig ExtraLifePowerUpConfig => _configurationValues.GameEntities.PowerUps.ExtraLifePowerUp;
        public static EffectPowerUpConfig FreezerPowerUpConfig => _configurationValues.GameEntities.PowerUps.FreezerPowerUp;
        #endregion

        #region Bullets Config
        public static LaserBulletConfig LaserBulletConfig => _configurationValues.GameEntities.Bullets.LaserBullet;
        #endregion

        #region Easy Enemies Config
        public static EnemyShipConfig BasicEnemyConfig => _configurationValues.GameEntities.Enemies.EasyEnemies.BasicEnemy;

        public static KamikazeEnemyConfig KamikazeEnemyConfig => _configurationValues.GameEntities.Enemies.EasyEnemies.KamikazeEnemy;
        #endregion

        #region MediumEnemiesConfig
        public static EnemyShipConfig BasicShooterEnemyConfig => _configurationValues.GameEntities.Enemies.MediumEnemies.BasicShooterEnemy;
        #endregion

        #endregion

        /// <summary>
        /// Initializes the configuration utils and get's the custom values from the json.
        /// </summary>
        public static void Initialize()
        {
            if (_alreadyInitialized)
                return;

            ForceInitialize();
        }

        /// <summary>
        /// Forces the initialization, possibly replacing previous values and references.
        /// </summary>
        public static void ForceInitialize()
        {
            _alreadyInitialized = true;
            try
            {
                //Parse File
                ConfigurationValues possibleNewValues = JsonConvert.DeserializeObject<ConfigurationValues>
                    (File.ReadAllText(Path.Combine(Application.streamingAssetsPath, _FILE_NAME)));

                _configurationValues = possibleNewValues ?? throw new Exception("The object returned from the file is null!");
            }
            catch (Exception e)
            {
                //Sets values to default.
                _configurationValues = _DEFAULT_CONFIGURATION_VALUES;

                //Prints the error message.
                Debug.Log($"An error ocurred while trying to get information from the {_FILE_NAME} file.");
                Debug.Log($"ERROR MESSAGE: {e.Message}");
            }
        }
    }
}