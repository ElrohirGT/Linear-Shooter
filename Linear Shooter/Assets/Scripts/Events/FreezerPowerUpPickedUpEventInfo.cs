using System;

namespace Events
{
    public struct FreezerPowerUpPickedUpEventInfo
    {
        public FreezerPowerUpPickedUpEventInfo(float effectDuration)
        {
            EffectDuration = effectDuration;
        }

        public float EffectDuration { get; }
    }
}
