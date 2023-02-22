using System.Collections.Generic;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/Mirror Movement")]
    public class MirrorMovementEffect : APowerUpEffect
    {
        private static readonly Dictionary<GameObject, RadialPlayerMovement> ReceiverCache = new();

        public override void Apply(GameObject player)
        {
            if (!ReceiverCache.TryGetValue(player, out var receiver))
            {
                receiver = player.transform.GetComponent<RadialPlayerMovement>();
                ReceiverCache.Add(player, receiver);
            }

            receiver.FlipMovementDirection();
        }

        public override void Remove(GameObject player)
        {
            ReceiverCache[player].ResetMovementDirection();
        }
    }
}