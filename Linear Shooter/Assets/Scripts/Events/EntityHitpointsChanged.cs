using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    public readonly struct EntityHitpointsChanged
    {
        public float RemainingHitpoints { get; }

        public float MaxHitpoints { get; }

        public EntityHitpointsChanged(float remainingHitpoints, float maxHitpoints)
        {
            RemainingHitpoints = remainingHitpoints;
            MaxHitpoints = maxHitpoints;
        }
    }
}