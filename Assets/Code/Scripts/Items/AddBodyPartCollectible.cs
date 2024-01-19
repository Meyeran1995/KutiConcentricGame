using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    public class AddBodyPartCollectible : ACollectible
    {
        public override void Collect(GameObject player)
        {
            var playerBody = FindAnyObjectByType<PlayerBody>(FindObjectsInactive.Exclude);
            playerBody.AcquireBodyPart();
        }
    }
}
