using Meyham.Animation;
using Meyham.DataObjects;
using Meyham.Player.Bodies;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerItemCollector : MonoBehaviour
    {
        [SerializeField] private PlayerBody playerBody;
        
        public void OnItemCollected(ACollectibleData item, ATweenBasedAnimation collectionAnimationHandle)
        {
            if (item is AddBodyPartCollectible)
            {
                if (playerBody.CanAcquireBodyParts())
                {
                    playerBody.AcquireBodyPartAnimated(collectionAnimationHandle);
                }
                
                return;
            }
            
            playerBody.LoseBodyPartAnimated(collectionAnimationHandle);
        }
    }
}
