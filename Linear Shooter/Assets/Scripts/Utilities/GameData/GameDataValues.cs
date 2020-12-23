using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Utilities.GameData
{
    public class DifficultyInfo
    {
        [JsonProperty("Name")]
        public string Name { get; }
        [JsonProperty("Description")]
        public string Description { get; }

        [JsonConstructor]
        public DifficultyInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
    public class ShipInfo
    {
        [JsonProperty("Name")]
        public string Name { get; }

        [JsonProperty("ClassName")]
        public string ClassName { get; }

        [JsonProperty("InitialLives")]
        public int InitialLives { get; }

        [JsonProperty("UltimateDescription")]
        public string UltimateDescription { get; }

        [JsonProperty("Unlocked")]
        public bool Unlocked { get; }

        [JsonConstructor]
        public ShipInfo(string name, string className, int initialLives, string ultimateDescription, bool unlocked)
        {
            Name = name;
            ClassName = className;
            InitialLives = initialLives;
            UltimateDescription = ultimateDescription;
            Unlocked = unlocked;
        }
    }

    public class GameDataValues
    {
        [JsonProperty("ShipInfo")]
        public ShipInfo[] ShipsInfo { get; }

        [JsonProperty("DifficultyInfo")]
        public DifficultyInfo[] DifficultiesInfo { get; }

        [JsonConstructor]
        public GameDataValues(ShipInfo[] shipsInfo, DifficultyInfo[] difficultiesInfo)
        {
            ShipsInfo = shipsInfo;
            DifficultiesInfo = difficultiesInfo;
        }
    }
}
