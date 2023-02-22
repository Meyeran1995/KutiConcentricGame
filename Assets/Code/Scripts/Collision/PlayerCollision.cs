using System;
using System.Collections;
using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollision : MonoBehaviour, IComparable<PlayerCollision>
    {
        [SerializeField] private FloatValue radius, collisionSizeFactor;
        [Header("Raycasts")]
        [SerializeField] private Transform localRaycastOriginLeft;
        [SerializeField] private Transform localRaycastOriginRight;
        [Header("References")]
        [SerializeField] private Collider playerCollider;
        [SerializeField] private PlayerVelocityCalculator velocityCalculator;
        
        [field: SerializeField] 
        public PlayerOrder PlayerOrder { get; private set; }

        [field: SerializeField]
        public bool AllowRaycast { get; private set; }
        
        public bool TriggerCollision { get; private set; }

        private float Velocity => velocityCalculator.LastVelocity;
        private int VelocityOrder => velocityCalculator.VelocityOrder;

        private static readonly Dictionary<Collider, PlayerCollision> ColliderToPlayer = new();
        private static Transform origin;
        private static LayerMaskProvider maskProvider;
        
        public void OnTriggerCollision()
        {
            PlayerOrder.IncrementOrder();
            TriggerCollision = false;
            StartCoroutine(TransitionDelay());
        }
        
        public void FireCollisionRaycasts(RaycastHit[] hits)
        {
            if(PlayerOrder.TransitionLocked)
            {
                return;
            }
            
            Vector3 originPos = origin.position;

            Vector3 raycastOrigin = localRaycastOriginLeft.position;
            int leftOrder = ComputeOrder(raycastOrigin, raycastOrigin - originPos, hits, Color.red);
            
            raycastOrigin = localRaycastOriginRight.position;
            int rightOrder = ComputeOrder(raycastOrigin, raycastOrigin - originPos, hits, Color.blue);
            
            int newOrder = Mathf.Max(leftOrder, rightOrder) + 1;
            
            if (newOrder == PlayerOrder.Order)
            {
                return;
            }
            
            raycastOrigin = transform.position;
            int middleOrder = ComputeOrder(raycastOrigin, raycastOrigin - originPos, hits, Color.green);
            
            newOrder = Mathf.Max(newOrder, middleOrder + 1);
            
            PlayerOrder.OrderPlayer(newOrder);
            StartCoroutine(TransitionDelay());
        }

        public void OnOrderChanged(int order)
        {
            gameObject.layer = maskProvider.GetLayer(order);
            var currentScale = transform.localScale;
            
            if (order == 0)
            {
                transform.localScale = new Vector3(1f, currentScale.y, currentScale.z);
                return;
            }

            transform.localScale = new Vector3(1f - collisionSizeFactor * order, currentScale.y, currentScale.z);
        }
        
        private int FireRaycast(Vector3 raycastOrigin, Vector3 raycastDirection, RaycastHit[] hits)
        {
            int mask = maskProvider.GetMask(PlayerOrder.Order);
            // returns size of buffer as in number of hits
            return Physics.RaycastNonAlloc(raycastOrigin, raycastDirection, hits, radius, mask);
        }
        
        private int GetOrderFromRaycastHits(RaycastHit[] hits, int length)
        {
            int order = -1;

            for (int i = 0; i < length; i++)
            {
                var collision = ColliderToPlayer[hits[i].collider];
                int hitOrder = collision.PlayerOrder.Order;

                if (hitOrder <= order) continue;

                order = hitOrder;
            }

            return order;
        }

        private int ComputeOrder(Vector3 raycastOrigin, Vector3 rayCastDirection, RaycastHit[] hits, Color debugColor)
        {
            int order = -1;
            int rayCastHits = FireRaycast(raycastOrigin, rayCastDirection, hits);
            
            if(rayCastHits != 0)
            {
                order = GetOrderFromRaycastHits(hits, rayCastHits);
                if (order >= 0)
                {
                    Debug.DrawRay(raycastOrigin, hits[0].point - raycastOrigin, debugColor,1f);
                }
            }
            else
            {
                Debug.DrawRay(raycastOrigin, rayCastDirection, debugColor,1f);
            }

            return order;
        }

        private IEnumerator TransitionDelay()
        {
            AllowRaycast = false;
            playerCollider.isTrigger = false;
            yield return new WaitWhile(() => PlayerOrder.TransitionLocked);
            AllowRaycast = PlayerOrder.Order > 0;
            playerCollider.isTrigger = true;
        }
        
        private void Awake()
        {
            ColliderToPlayer.Add(playerCollider, this);

            maskProvider ??= new LayerMaskProvider();
            origin ??= GameObject.FindGameObjectWithTag("Origin").transform;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(TriggerCollision) return;
            
            var playerCollision = ColliderToPlayer[other];
            
            int collisionVelocityOrder = playerCollision.VelocityOrder;

            if(collisionVelocityOrder > VelocityOrder) return;
            
            if(collisionVelocityOrder == VelocityOrder && playerCollision.Velocity > Velocity) return;

            playerCollision.TriggerCollision = true;
        }

        private void OnDisable()
        {
            AllowRaycast = false;
            TriggerCollision = false;
            StopAllCoroutines();
        }

        public int CompareTo(PlayerCollision other)
        {
            return PlayerOrder.Order.CompareTo(other.PlayerOrder.Order);
        }
    }
}