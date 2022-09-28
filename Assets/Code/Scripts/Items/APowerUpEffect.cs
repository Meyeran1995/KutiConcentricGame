using UnityEngine;

namespace Meyham.Items
{
    public abstract class APowerUpEffect : ScriptableObject
    {
        public bool HasSingleTarget { get; }

        public float Duration { get; }

        public abstract void Apply(GameObject player);

        public abstract void Remove(GameObject player);
    }
}