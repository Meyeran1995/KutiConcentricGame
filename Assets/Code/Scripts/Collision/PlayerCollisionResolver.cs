using System.Collections.Generic;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollisionResolver : MonoBehaviour
    {
        private RaycastHit[] hits;
        private bool fixedRan;
        
        public static readonly List<PlayerCollision> PlayerCollisions = new();

        private void Awake()
        {
            hits = new RaycastHit[5];
        }

        private void FixedUpdate()
        {
            fixedRan = true;
        }

        private void LateUpdate()
        {
            if(!fixedRan) return;

            fixedRan = false;
            PlayerCollisions.Sort();

            foreach (var collision in PlayerCollisions)
            {
                if (collision.TriggerCollision)
                {
                    collision.OnTriggerCollision();
                    continue;
                }

                if (!collision.AllowRaycast) continue;
                
                collision.FireCollisionRaycasts(hits);
            }

            foreach (var collision in PlayerCollisions)
            {
                collision.PlayerOrder.UpdatePlayerOrder();
            }
        }
    }
}