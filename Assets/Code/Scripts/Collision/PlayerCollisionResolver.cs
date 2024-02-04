using System;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollisionResolver : MonoBehaviour, IPlayerNumberDependable
    {
        private RaycastHit[] hits;
        
        private PlayerCollision[] playerCollisions;

        private int playerCount;

        public void OnPlayerJoined(int playerNumber)
        {
            playerCount++;
        }

        public void OnPlayerLeft(int playerNumber)
        {
            playerCount--;
        }
        
        public void SetPlayerCollisions(PlayerCollision[] collisions)
        {
            playerCollisions = collisions;
        }
        
        private void OnEnable()
        {
            int maximumCollisions = playerCount - 1;
            
            if (maximumCollisions <= 0)
            {
                enabled = false;
                return;
            }
            
            hits = new RaycastHit[maximumCollisions];
        }

        private void OnDisable()
        {
            hits = null;
        }
        
        private void FixedUpdate()
        {
            Array.Sort(playerCollisions);

            foreach (var collision in playerCollisions)
            {
                collision.OnStartCollisionChecks();
                
                collision.ResolveForwardCollisions(hits);
                collision.ResolveDownwardChecks(hits);
                
                collision.OnFinishedCollisionChecks();
            }
        }
    }
}