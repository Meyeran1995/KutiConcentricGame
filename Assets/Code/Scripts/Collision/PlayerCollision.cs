using System;
using System.Collections;
using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.Player;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollision : MonoBehaviour, IComparable<PlayerCollision>
    {
        [SerializeField] private FloatValue radius;
        [Header("Raycasts")]
        [SerializeField] private Transform localRaycastOriginLeft;
        [SerializeField] private Transform localRaycastOriginRight;
        [Header("References")]
        [SerializeField] private Collider playerCollider;
        [SerializeField] private PlayerVelocityCalculator velocityCalculator;
        [SerializeField] private PlayerOrder playerOrder;

        public bool triggerCollision, allowRaycast;
        
        private float Velocity => velocityCalculator.LastVelocity;
        private int VelocityOrder => velocityCalculator.VelocityOrder;

        private static readonly Dictionary<Collider, PlayerCollision> ColliderToPlayer = new();
        private static Transform origin;

        //private List<Collider> collisions;

        public void OnTriggerCollision()
        {
            playerOrder.IncrementOrder();
            playerCollider.isTrigger = false;
            triggerCollision = false;
            StartCoroutine(Wait());
        }
        
        public void FireCollisionRaycasts()
        {
            gameObject.layer = 2;
            
            RaycastHit[] hits = new RaycastHit[PlayerManager.PlayerCount - 1];
            Vector3 originPos = origin.position;
            int leftOrder = -1;
            int rightOrder = -1;

            Vector3 rayCastOrigin = localRaycastOriginLeft.position;
            
            if (FireRaycast(rayCastOrigin, rayCastOrigin - originPos,  hits))
            {
                leftOrder = GetOrderFromHits(hits);
            }
            
            rayCastOrigin = localRaycastOriginRight.position;
            
            if(FireRaycast(rayCastOrigin, rayCastOrigin - originPos,  hits))
            {
                rightOrder = GetOrderFromHits(hits);
            }
            
            int newOrder = Mathf.Max(leftOrder, rightOrder) + 1;
            
            gameObject.layer = 6;
            
            if(newOrder == playerOrder.Order) return;
            
            playerOrder.OrderPlayer(newOrder);
            //collisions.RemoveAll(item => ColliderToPlayer[item].playerOrder.Order != newOrder);
            playerCollider.isTrigger = newOrder == 0;
            allowRaycast = newOrder != 0;
            
            if(newOrder == 0) return;
            
            StartCoroutine(Wait());
        }

        private bool FireRaycast(Vector3 raycastOrigin, Vector3 raycastDirection, RaycastHit[] hits)
        {
            const int mask = 1 << 6;
            raycastDirection.Normalize();
            Debug.DrawRay(raycastOrigin, raycastDirection * radius, Color.red,1f);
            return Physics.RaycastNonAlloc(raycastOrigin, raycastDirection, hits, radius, mask) != 0;
        }
        
        private int GetOrderFromHits(RaycastHit[] hits)
        {
            int order = -1;
            
            foreach (var hit in hits)
            {
                var hitCollider = hit.collider;
                
                if(hitCollider == null) continue;
                
                var collision = ColliderToPlayer[hitCollider];
                int hitOrder = collision.playerOrder.Order;
                
                if(hitOrder <= order) continue;

                order = hitOrder;
            }

            return order;
        }
        
        private void Awake()
        {
            PlayerCollisionResolver.PlayerCollisions.Add(this);
            //collisions = new List<Collider>(5);
            ColliderToPlayer[playerCollider] = this;
            
            if(origin != null) return;

            origin = GameObject.FindGameObjectWithTag("Origin").transform;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            //if (collisions.Contains(other)) return;

            var playerCollision = ColliderToPlayer[other];
            
            if(playerCollision.playerOrder.Order != playerOrder.Order) return;
            
            int collisionVelocityOrder = playerCollision.VelocityOrder;

            if(collisionVelocityOrder > VelocityOrder) return;
            
            if(collisionVelocityOrder == VelocityOrder && playerCollision.Velocity > Velocity) return;
            
            playerCollision.IncomingCollision(playerCollider);
        }

        // private void OnTriggerExit(Collider other)
        // {
        //     if(!collisions.Contains(other)) return;
        //     collisions.Remove(other);
        // }

        private void IncomingCollision(Collider other)
        {
            // if(collisions.Contains(other)) return;
            // collisions.Add(other);
            //playerOrder.IncrementOrder();
            //playerCollider.isTrigger = false;
            triggerCollision = true;
            //StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            allowRaycast = false;
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();
            allowRaycast = true;
        }

        // private void FixedUpdate()
        // {
        //     if(!allowRaycast) return;
        //     
        //     FireCollisionRaycasts();
        // }

        public int CompareTo(PlayerCollision other)
        {
            return other.playerOrder.Order.CompareTo(playerOrder.Order);
        }
    }
}