using System;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollisionResolver : MonoBehaviour
    {
        private RaycastHit[] hits;
        
        private PlayerCollision[] playerCollisions;
        
        private void Awake()
        {
            hits = new RaycastHit[5];
            enabled = false;
        }

        private void OnEnable()
        {
            hits = new RaycastHit[5];
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
                if (collision.TriggerCollision)
                {
                    collision.OnTriggerCollision();
                    continue;
                }

                if (!collision.AllowRaycast) continue;
                
                collision.FireCollisionRaycasts(hits);
            }

            foreach (var collision in playerCollisions)
            {
                collision.PlayerOrder.UpdatePlayerOrder();
            }
        }
    }
}