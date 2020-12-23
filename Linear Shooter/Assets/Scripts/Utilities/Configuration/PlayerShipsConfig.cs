using System.Collections.Generic;
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;

namespace Utilities.Configuration
{
    public partial class PlayerShipsConfig
    {
        [JsonConstructor]
        public PlayerShipsConfig(StandardShipConfig standardShip, ShipConfig grenadierShip, ShipConfig tankShip)
        {
            StandardShip = standardShip;
            GrenadierShip = grenadierShip;
            TankShip = tankShip;
        }

        [J("StandardShip", Required = R.Always)] public StandardShipConfig StandardShip { get; }
        [J("GrenadierShip", Required = R.Always)] public ShipConfig GrenadierShip { get; }
        [J("TankShip", Required = R.Always)] public ShipConfig TankShip { get; }
    }
    public class PlayerShipConfig : ShipConfig
    {
        [JsonConstructor]
        public PlayerShipConfig(int minMedalsToUltimate, float rotationSpeed, float thrustAmount, Dictionary<string, GunConfig> guns, float maxHitpoints, float initialHitpoints, float baseDamage, float damageCooldownDuration)
            : base(rotationSpeed, thrustAmount, guns, maxHitpoints, initialHitpoints, baseDamage, damageCooldownDuration)
        {
            MinMedalsToUltimate = minMedalsToUltimate;
        }

        [J("MinMedalsToUltimate", Required = R.Always)] public int MinMedalsToUltimate { get; }
    }

    public class StandardShipConfig : PlayerShipConfig
    {
        [JsonConstructor]
        public StandardShipConfig(float ultimateDuration, int minMedalsToUltimate, float rotationSpeed, float thrustAmount, Dictionary<string, GunConfig> guns, float maxHitpoints, float initialHitpoints, float baseDamage, float damageCooldownDuration)
            : base(minMedalsToUltimate, rotationSpeed, thrustAmount, guns, maxHitpoints, initialHitpoints, baseDamage, damageCooldownDuration)
        {
            UltimateDuration = ultimateDuration;
        }

        [J("UltimateDuration")] public float UltimateDuration { get; }
    }
}