using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;

namespace Utilities.Configuration
{
    public partial class WavesConfig
    {
        [JsonConstructor]
        public WavesConfig(DifficultyWavesConfig easyWaves, DifficultyWavesConfig mediumWaves, DifficultyWavesConfig hardWaves)
        {
            EasyWaves = easyWaves;
            MediumWaves = mediumWaves;
            HardWaves = hardWaves;
        }

        [J("EasyWaves", Required = R.Always)] public DifficultyWavesConfig EasyWaves { get; }
        [J("MediumWaves", Required = R.Always)] public DifficultyWavesConfig MediumWaves { get; }
        [J("HardWaves", Required = R.Always)] public DifficultyWavesConfig HardWaves { get; }
    }

    public partial class DifficultyWavesConfig
    {
        [JsonConstructor]
        public DifficultyWavesConfig(WaveConfig easyEnemies, WaveConfig mediumEnemies, WaveConfig hardEnemies, WaveConfig boss)
        {
            EasyEnemies = easyEnemies;
            MediumEnemies = mediumEnemies;
            HardEnemies = hardEnemies;
            Boss = boss;
        }

        [J("EasyEnemies", Required = R.Always)] public WaveConfig EasyEnemies { get; }
        [J("MediumEnemies", Required = R.Always)] public WaveConfig MediumEnemies { get; }
        [J("HardEnemies", Required = R.Always)] public WaveConfig HardEnemies { get; }
        [J("Boss", Required = R.Always)] public WaveConfig Boss { get; }
    }

    public partial class WaveConfig
    {
        [JsonConstructor]
        public WaveConfig(float[] pattern, float growthValue, WaveCheckPoint[] checkPoints)
        {
            Pattern = pattern;
            GrowthValue = growthValue;
            CheckPoints = checkPoints;
        }

        [J("Pattern", Required = R.Always)] public float[] Pattern { get; }
        [J("GrowthValue", Required = R.Always)] public float GrowthValue { get; }
        [J("CheckPoints", Required = R.Always)] public WaveCheckPoint[] CheckPoints { get; }
    }

    public partial class WaveCheckPoint
    {
        [JsonConstructor]
        public WaveCheckPoint(int waveNumber, int quantityToSpawn)
        {
            WaveNumber = waveNumber;
            QuantityToSpawn = quantityToSpawn;
        }

        [J("WaveNumber", Required = R.Always)] public int WaveNumber { get; }
        [J("QuantityToSpawn", Required = R.Always)] public int QuantityToSpawn { get; }
    }
}
