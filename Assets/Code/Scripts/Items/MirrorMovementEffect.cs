using System.Collections.Generic;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/Mirror Movement")]
    public class MirrorMovementEffect : APowerUpEffect
    {
        private static readonly Dictionary<GameObject, PlayerInputReceiver> receiverCache = new();

        public override void Apply(GameObject player)
        {
            if (!receiverCache.TryGetValue(player, out var receiver))
            {
                receiver = player.transform.GetComponent<PlayerInputReceiver>();
                receiverCache.Add(player, receiver);
            }

            receiver.FlipMovementDirection();
        }

        public override void Remove(GameObject player)
        {
            receiverCache[player].FlipMovementDirection();
        }
    }
}