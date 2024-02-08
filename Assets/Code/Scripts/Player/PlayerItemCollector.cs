using Meyham.Animation;
using Meyham.DataObjects;
using Meyham.Player.Bodies;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerItemCollector : MonoBehaviour
    {
        [SerializeField] private PlayerBody playerBody;
        
        public void OnItemCollected(ACollectibleData item, AddBodyCollectionAnimationHandle collectionAnimationHandle = null)
        {
            if (item is AddBodyPartCollectible && playerBody.CanAcquireBodyParts())
            {
                playerBody.AcquireBodyPartAnimated(collectionAnimationHandle);
                return;
            }
            
            playerBody.LoseBodySegment();
        }
    }
}
