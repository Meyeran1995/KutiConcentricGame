using Meyham.Player;
using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/Collectibles/DestroyBodyPart")]
    public class DestroyBodyPartCollectible : ACollectibleData
    {
        public override void Collect(GameObject player)
        {
            var playerBody = FindAnyObjectByType<PlayerBody>(FindObjectsInactive.Exclude);
            playerBody.LoseBodySegment();
        }
    }
}