using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    public readonly struct PlayerHitpointsChangedEventInfo
    {

        public int RemainingLives { get; }

        public PlayerHitpointsChangedEventInfo(int remainingLives)
        {
            RemainingLives = remainingLives;
        }
    }
}