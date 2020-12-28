using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Utilities.GameData
{
    /// <summary>
    /// Serves as the database to the whole game.
    /// </summary>
    public static class GameDataUtils
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
        /// Get's the name of the file that has all the game data.
        /// </summary>
        const string _FILE_NAME = "GameData.json";

        //TODO Complete de GameData default values
        /// <summary>
        /// The defaults game data values in case the file doesn't exists or it fails to read it.
        /// </summary>
        static readonly GameDataValues _DEFAULT_GAME_DATA_VALUES;

        /// <summary>
        /// Determines whether this class has or hasn't been initialized.
        /// </summary>
        static bool _alreadyInitialized = false;

        /// <summary>
        /// Has all the game data.
        /// </summary>
        static GameDataValues _gameDataValues;

        /* 
        .#####...#####....####...#####...######..#####...######..######..######...####..
        .##..##..##..##..##..##..##..##..##......##..##....##......##....##......##.....
        .#####...#####...##..##..#####...####....#####.....##......##....####.....####..
        .##......##..##..##..##..##......##......##..##....##......##....##..........##.
        .##......##..##...####...##......######..##..##....##....######..######...####..
        ................................................................................
        */

        static string FilePath => Path.Combine(Application.streamingAssetsPath, _FILE_NAME);

        /// <summary>
        /// Get's the information of the ships.
        /// </summary>
        public static Dictionary<string, ShipInfo> ShipsInfo => _gameDataValues.ShipsInfo;

        /// <summary>
        /// Get's the information of the difficulties in the game.
        /// </summary>
        public static DifficultyInfo[] DifficultiesInfo => _gameDataValues.DifficultiesInfo;

        public static PlayerInfo PlayerInfo => _gameDataValues.PlayerInfo;

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
                //TODO make read it from a binary file.
                //Parse file, this could be in binary in the future
                GameDataValues possibleNewValues = ParseFile();

                _gameDataValues = possibleNewValues ?? throw new Exception("The object returned from the file is null!");
            }
            catch (Exception e)
            {
                //Sets values to default.
                _gameDataValues = _DEFAULT_GAME_DATA_VALUES;

                //Prints the error message.
                Debug.Log($"An error ocurred while trying to get information from the {_FILE_NAME} file.");
                Debug.Log($"ERROR MESSAGE: {e.Message}");
            }
        }

        /// <summary>
        /// Saves the current state of the game data to the file.
        /// </summary>
        public static void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_gameDataValues);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception) { };
        }

        /// <summary>
        /// Parses the file that has the GameData.
        /// </summary>
        /// <returns>An instance of GameDataValues</returns>
        /// <exception cref="Exception">Thrown when the parsing fails.</exception>
        static GameDataValues ParseFile()
        {
            try
            {
                return JsonConvert.DeserializeObject<GameDataValues>
                    (File.ReadAllText(FilePath));
            }
            catch (Exception) { throw; }
        }
    }
}
