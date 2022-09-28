using UnityEngine;

namespace Meyham.Items
{
    public abstract class APowerUpEffect : ScriptableObject
    {
        [field: SerializeField]
        public bool HasSingleTarget { get; protected set; }
        
        [field: SerializeField]
        public float Duration { get; protected set; }

        public abstract void Apply(GameObject player);

        public abstract void Remove(GameObject player);
    }
}