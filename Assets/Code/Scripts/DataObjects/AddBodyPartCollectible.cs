using Meyham.Player;
using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/Collectibles/AddBodyPart")]
    public class AddBodyPartCollectible : ACollectibleData
    {
        public override void Collect(GameObject player)
        {
            var playerBody = FindAnyObjectByType<PlayerBody>(FindObjectsInactive.Exclude);
            playerBody.AcquireBodyPart();
        }
    }
}
