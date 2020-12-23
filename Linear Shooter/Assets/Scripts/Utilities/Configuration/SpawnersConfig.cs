using System.Collections.Generic;
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;

namespace Utilities.Configuration
{
    public partial class SpawnersConfig
    {
        [JsonConstructor]
        public SpawnersConfig(TimedSpawnerConfig medalSpawner, WaveSpawnerConfig easyEnemiesSpawner, WaveSpawnerConfig mediumEnemiesSpawner, ProbabilitySpawnerConfig powerUpSpawner)
        {
            MedalSpawner = medalSpawner;
            EasyEnemiesSpawner = easyEnemiesSpawner;
            MediumEnemiesSpawner = mediumEnemiesSpawner;
            PowerUpSpawner = powerUpSpawner;
        }

        [J("MedalSpawner", Required = R.Always)] public TimedSpawnerConfig MedalSpawner { get; }
        [J("EasyEnemiesSpawner", Required = R.Always)] public WaveSpawnerConfig EasyEnemiesSpawner { get; }
        [J("MediumEnemiesSpawner", Required = R.Always)] public WaveSpawnerConfig MediumEnemiesSpawner { get; }
        [J("PowerUpSpawner", Required = R.Always)] public ProbabilitySpawnerConfig PowerUpSpawner { get; }
    }

    public partial class WaveSpawnerConfig
    {
        [JsonConstructor]
        public WaveSpawnerConfig(float spawnCooldownDuration, float waveCooldownDuration, Dictionary<string, float> probabilitiesByTypesNames)
        {
            SpawnCooldownDuration = spawnCooldownDuration;
            WaveCooldownDuration = waveCooldownDuration;
            ProbabilitiesByTypesNames = probabilitiesByTypesNames;
        }
        [J("SpawnCooldownDuration", Required = R.Always)] public float SpawnCooldownDuration { get; }
        [J("WaveCooldownDuration", Required = R.Always)] public float WaveCooldownDuration { get; }
        [J("Probabilities", Required = R.Always)] public Dictionary<string, float> ProbabilitiesByTypesNames { get; }
    }

    public partial class TimedSpawnerConfig
    {
        [JsonConstructor]
        public TimedSpawnerConfig(float initialDelay, float minSpawnDelay, float maxSpawnDelay)
        {
            InitialDelay = initialDelay;
            MinSpawnDelay = minSpawnDelay;
            MaxSpawnDelay = maxSpawnDelay;
        }

        [J("InitialDelay", Required = R.Always)] public float InitialDelay { get; }
        [J("MinSpawnDelay", Required = R.Always)] public float MinSpawnDelay { get; }
        [J("MaxSpawnDelay", Required = R.Always)] public float MaxSpawnDelay { get; }
    }
    public class ProbabilitySpawnerConfig : TimedSpawnerConfig
    {
        [JsonConstructor]
        public ProbabilitySpawnerConfig(float initialDelay, float minSpawnDelay, float maxSpawnDelay, Dictionary<string, float> probabilitiesByTypesNames) : base(initialDelay, minSpawnDelay, maxSpawnDelay)
        {
            ProbabilitiesByTypesNames = probabilitiesByTypesNames;
        }

        [J("Probabilities", Required = R.Always)] public Dictionary<string, float> ProbabilitiesByTypesNames { get; }
    }
}