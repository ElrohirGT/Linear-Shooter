using System;

namespace Events{

    public readonly struct PlayerDiedEventInfo
    {
        public float Score { get; }

        public PlayerDiedEventInfo(float score)
        {
            Score = score;
        }

    }
}
