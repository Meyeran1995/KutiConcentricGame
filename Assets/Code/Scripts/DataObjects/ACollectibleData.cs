using System.Collections.Generic;
using Meyham.Player;
using Meyham.Player.Bodies;
using UnityEngine;

namespace Meyham.DataObjects
{
    public abstract class ACollectibleData : ScriptableObject
    {
        private static readonly Dictionary<GameObject, PlayerItemCollector> PlayerItemCollectors = new();
        
        public void Collect(GameObject bodyPart)
        {
            if (PlayerItemCollectors.TryGetValue(bodyPart, out var collector))
            {
                collector.OnItemCollected(this);
                return;
            }

            collector = bodyPart.GetComponent<PlayerItemCollector>();
            collector.OnItemCollected(this);
            PlayerItemCollectors.Add(bodyPart, collector);
        }
    }
}