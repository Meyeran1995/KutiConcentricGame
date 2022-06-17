using Meyham.Items;
using UnityEngine;

namespace Meyham.GameMode
{
    public class CollectibleSpawner : MonoBehaviour
    {
        public void ReleaseCollectible(ACollectible collectible)
        {
            Debug.Log($"{collectible.name} has touched the border");
        }
    }
}
