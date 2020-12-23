using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events{
    public readonly struct EnemyDiedEventInfo {

        public float PointsWorth { get; }

        public EnemyDiedEventInfo(float pointsWorth)
        {
            PointsWorth = pointsWorth;
        }
    }
}