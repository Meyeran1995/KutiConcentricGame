using System;
using System.Collections;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
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
        [SerializeField] private PlayerOrder playerOrder;

        [field: Header("Debug"), SerializeField, ReadOnly]
        public bool AllowRaycast { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public bool TriggerCollision { get; private set; }

        private float Velocity => velocityCalculator.LastVelocity;
        
        private int VelocityOrder => velocityCalculator.VelocityOrder;

        public void OnTriggerCollision()
        {
            playerOrder.IncrementOrder();
            TriggerCollision = false;
            StartCoroutine(TransitionDelay());
        }
        
        public void FireCollisionRaycasts(RaycastHit[] hits)
        {
            if(playerOrder.TransitionLocked)
            {
                return;
            }
            
            Vector3 originPos = PlayerCollisionHelper.GetOriginPos();

            Vector3 raycastOrigin = localRaycastOriginLeft.position;
            int leftOrder = ComputeOrder(raycastOrigin, raycastOrigin - originPos, hits, Color.red);
            
            raycastOrigin = localRaycastOriginRight.position;
            int rightOrder = ComputeOrder(raycastOrigin, raycastOrigin - originPos, hits, Color.blue);
            
            int newOrder = Mathf.Max(leftOrder, rightOrder) + 1;
            
            if (newOrder == playerOrder.Order)
            {
                return;
            }
            
            raycastOrigin = transform.position;
            int middleOrder = ComputeOrder(raycastOrigin, raycastOrigin - originPos, hits, Color.green);
            
            newOrder = Mathf.Max(newOrder, middleOrder + 1);
            
            playerOrder.OrderPlayer(newOrder);
            StartCoroutine(TransitionDelay());
        }

        public void OnOrderChanged(int order)
        {
            gameObject.layer = PlayerCollisionHelper.GetLayer(order);
            var currentScale = transform.localScale;
            
            if (order == 0)
            {
                transform.localScale = new Vector3(1f, currentScale.y, currentScale.z);
                return;
            }

            transform.localScale = new Vector3(1f - collisionSizeFactor * order, currentScale.y, currentScale.z);
        }

        public void UpdateOrder()
        {
            playerOrder.UpdatePlayerOrder();
        }

        private int FireRaycast(Vector3 raycastOrigin, Vector3 raycastDirection, RaycastHit[] hits)
        {
            int mask = PlayerCollisionHelper.GetMask(playerOrder.Order);
            // returns size of buffer as in number of hits
            return Physics.RaycastNonAlloc(raycastOrigin, raycastDirection, hits, radius, mask);
        }
        
        private int GetOrderFromRaycastHits(RaycastHit[] hits, int length)
        {
            int order = -1;

            for (int i = 0; i < length; i++)
            {
                var collision = PlayerCollisionHelper.GetPlayerByCollider(hits[i].collider);
                int hitOrder = collision.playerOrder.Order;

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
            yield return new WaitWhile(() => playerOrder.TransitionLocked);
            AllowRaycast = playerOrder.Order > 0;
            playerCollider.isTrigger = true;
        }
        
        private void Awake()
        {
            PlayerCollisionHelper.Register(playerCollider, this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(TriggerCollision) return;

            var playerCollision = PlayerCollisionHelper.GetPlayerByCollider(other);
            
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
            return playerOrder.Order.CompareTo(other.playerOrder.Order);
        }
    }
}