using System;
using System.Collections.Generic;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollisionResolver : MonoBehaviour
    {
        private static readonly List<Tuple<PlayerOrder, PlayerOrder>> pendingCollisions = new ();
        private static readonly List<Tuple<PlayerOrder, PlayerOrder>> pendingExits = new ();
        
        public static void AddCollision(PlayerOrder player, Collider otherPlayer)
        {
            pendingCollisions.Add(new Tuple<PlayerOrder, PlayerOrder>(player, otherPlayer.GetComponent<PlayerOrder>()));
        }
        
        public static void AddExit(PlayerOrder player, Collider otherPlayer)
        {
            pendingExits.Add(new Tuple<PlayerOrder, PlayerOrder>(player, otherPlayer.GetComponent<PlayerOrder>()));
        }

        private void Update()
        {
            int collisionCount = pendingCollisions.Count;
            int exitCount = pendingExits.Count;

            switch (exitCount)
            {
                case 0 when collisionCount == 0:
                    return;
                case > 0:
                {
                    for (int i = exitCount - 1; i >= 0; i--)
                    {
                        pendingExits[i].Item1.DecrementOrder();
                    }

                    break;
                }
            }
            
            pendingExits.Clear();

            if (collisionCount == 0) return;
            
            for (int i = collisionCount - 1; i >= 0; i--)
            {
                var player1 = pendingCollisions[i].Item1;
                var player2 = pendingCollisions[i].Item2;

                if (player1.PlayerVelocity >= player2.PlayerVelocity)
                {
                    player2.IncrementOrder();
                    continue;
                }
                player1.IncrementOrder();
            }
            
            pendingCollisions.Clear();
        }
    }
}