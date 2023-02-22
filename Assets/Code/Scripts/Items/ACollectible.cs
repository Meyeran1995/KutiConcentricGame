using UnityEngine;

namespace Meyham.Items
{
    public abstract class ACollectible : MonoBehaviour
    {
        public abstract void Collect(GameObject player);
    }
}