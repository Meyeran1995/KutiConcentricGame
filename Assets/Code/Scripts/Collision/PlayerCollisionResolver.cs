using System.Collections.Generic;
using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollisionResolver : AGameLoopSystem
    {
        private RaycastHit[] hits;
        
        public static readonly List<PlayerCollision> PlayerCollisions = new();
        
        private void Awake()
        {
            hits = new RaycastHit[5];
        }

        private void FixedUpdate()
        {
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

        protected override void Start()
        {
            base.Start();
            enabled = false;
        }

        protected override void OnGameStart()
        {
            enabled = true;
        }

        protected override void OnGameEnd()
        {
            enabled = false;
        }

        protected override void OnGameRestart()
        {
            hits = new RaycastHit[5];
            enabled = true;
        }
    }
}