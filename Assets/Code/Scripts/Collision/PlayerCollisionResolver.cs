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
        
        private void OnEnable()
        {
            int maximumCollisions = playerCount - 1;
            
            if (maximumCollisions == 0)
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

        public void SetPlayerCollisions(PlayerCollision[] collisions)
        {
            playerCollisions = collisions;
        }
        
        private void FixedUpdate()
        {
            Array.Sort(playerCollisions);

            foreach (var collision in playerCollisions)
            {
                if(collision.AllowForwardRaycast)
                {
                    collision.FireRayInMovementDirection(hits);
                
                    if (collision.ForwardCollision)
                    {                
                        collision.ResolveSameOrderCollision(hits);
                        collision.UpdateOrder();
                        continue;
                    }
                }
                
                if (!collision.AllowDownwardRaycast) continue;
                
                collision.FireDownwardCollisionRaycasts(hits);
                collision.UpdateOrder();
            }
        }
    }
}