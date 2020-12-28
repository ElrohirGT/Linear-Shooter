using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Utilities.GameData
{
    public class PlayerInfo
    {
        [JsonConstructor]
        public PlayerInfo(float highscore)
        {
            Highscore = highscore;
        }

        [JsonProperty("Highscore")]
        public float Highscore { get; private set; }

        public void ChangeHighscore(float newHighscore)
        {
            Highscore = newHighscore;
        }
    }
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
        public string DisplayName { get; }

        [JsonProperty("InitialLives")]
        public int InitialLives { get; }

        [JsonProperty("UltimateDescription")]
        public string UltimateDescription { get; }

        [JsonProperty("Unlocked")]
        public bool Unlocked { get; private set; }

        [JsonProperty("ToUnlock")]
        public string ToUnlockMessage { get; }

        [JsonConstructor]
        public ShipInfo(string displayName, int initialLives, string ultimateDescription, bool unlocked, string toUnlockMessage)
        {
            DisplayName = displayName;
            InitialLives = initialLives;
            UltimateDescription = ultimateDescription;
            Unlocked = unlocked;
            ToUnlockMessage = toUnlockMessage;
        }

        public void Unlock() { Unlocked = true; }
    }

    public class GameDataValues
    {
        [JsonProperty("PlayerShips")]
        public Dictionary<string, ShipInfo> ShipsInfo { get; }

        [JsonProperty("Difficulties")]
        public DifficultyInfo[] DifficultiesInfo { get; }

        [JsonProperty("Player")]
        public PlayerInfo PlayerInfo { get; }

        [JsonConstructor]
        public GameDataValues(PlayerInfo playerInfo, Dictionary<string, ShipInfo> shipsInfo, DifficultyInfo[] difficultiesInfo)
        {
            ShipsInfo = shipsInfo;
            DifficultiesInfo = difficultiesInfo;
            PlayerInfo = playerInfo;
        }
    }
}
