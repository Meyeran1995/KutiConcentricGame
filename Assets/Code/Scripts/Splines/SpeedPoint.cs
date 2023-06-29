using System;
using UnityEngine;

namespace Meyham.Splines
{
    [Serializable]
    public struct SpeedPoint
    {
        [field: SerializeField, Range(0f,1f)]
        public float Progress { get; private set; }
        
        [field: SerializeField, Min(0f)]
        public float Modifier { get; private set; }

        public SpeedPoint(float progress, float modifier)
        {
            Progress = progress;
            Modifier = modifier;
        }
    }
}
