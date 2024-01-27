using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerItemCollector : MonoBehaviour
    {
        [SerializeField] private PlayerBody playerBody;

        public void OnItemCollected(ACollectibleData item)
        {
            if (item is AddBodyPartCollectible)
            {
                playerBody.AcquireBodyPart();
                return;
            }
            
            playerBody.LoseBodySegment();
        }
    }
}
