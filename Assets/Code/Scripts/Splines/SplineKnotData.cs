using System;
using UnityEngine.Splines;

namespace Meyham.Splines
{
    [Serializable]
    public readonly struct SplineKnotData
    {
        public readonly BezierKnot[] Knots;
        public readonly TangentMode[] TangentModes;

        public SplineKnotData(BezierKnot[] knots, TangentMode[] tangentModes)
        {
            Knots = knots;
            TangentModes = tangentModes;
        }
    }
}
