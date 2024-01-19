using UnityEngine;

namespace Meyham.DataObjects
{
    public abstract class ACollectibleData : ScriptableObject
    {
        public abstract void Collect(GameObject player);
    }
}