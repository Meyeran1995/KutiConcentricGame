using System.Collections.Generic;
using Meyham.Player;
using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/Collectibles/AddBodyPart")]
    public class AddBodyPartCollectible : ACollectibleData
    {
        private static readonly Dictionary<GameObject, PlayerItemCollector> PlayerItemCollectors = new();
        
        public override void Collect(GameObject player)
        {
            if (PlayerItemCollectors.TryGetValue(player, out var collector))
            {
                collector.OnItemCollected(this);
                return;
            }

            collector = player.GetComponent<PlayerItemCollector>();
            collector.OnItemCollected(this);
            PlayerItemCollectors.Add(player, collector);
        }
    }
}
