using Meyham.DataObjects;
using Unity.Collections;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemCollectibleCarrier : MonoBehaviour
    {
        [field: ReadOnly, SerializeField] 
        public ACollectibleData Collectible { get; private set; }

        public void SetCollectible(ACollectibleData collectibleData)
        {
            Collectible = collectibleData;
        }

        public void OnCollected(GameObject playerItemCollision)
        {
            Collectible.Collect(playerItemCollision);
        }
    }
}