using System.Collections.Generic;
using Meyham.Animation;
using Meyham.Items;
using Meyham.Player;
using UnityEngine;

namespace Meyham.DataObjects
{
    public abstract class ACollectibleData : ScriptableObject
    {
        private static readonly Dictionary<GameObject, PlayerItemCollector> PlayerItemCollectors = new();
        
        public void Collect(GameObject playerBody, ATweenBasedAnimation itemCollectionAnimationHandle)
        {
            if (PlayerItemCollectors.TryGetValue(playerBody, out var collector))
            {
                collector.OnItemCollected(this, itemCollectionAnimationHandle);
                return;
            }

            collector = playerBody.GetComponent<PlayerItemCollector>();
            collector.OnItemCollected(this, itemCollectionAnimationHandle);
            PlayerItemCollectors.Add(playerBody, collector);
        }
    }
}