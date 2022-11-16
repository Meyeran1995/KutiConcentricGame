using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollisionResolver : MonoBehaviour
    {
        public static readonly List<PlayerCollision> PlayerCollisions = new();

        private void FixedUpdate()
        {
            PlayerCollisions.Sort();

            foreach (var collision in PlayerCollisions)
            {
                if (collision.triggerCollision)
                {
                    collision.OnTriggerCollision();
                    continue;
                }

                if (!collision.allowRaycast) continue;
                
                collision.FireCollisionRaycasts();
            }
        }
    }
}