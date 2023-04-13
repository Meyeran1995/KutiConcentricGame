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
        [SerializeField] private FloatParameter radius, collisionSizeFactor;
        [Header("Raycast Origins")]
        [SerializeField] private Vector2 raycastOriginForward;
        [SerializeField] private Vector2 raycastOriginDownward;
        [Header("Raycast Direction")]
        [SerializeField] private Vector2 raycastDirectionForward;
        [SerializeField] private Vector2 raycastDirectionDownward;
        [Header("References")]
        [SerializeField] private Collider playerCollider;
        [SerializeField] private PlayerVelocityCalculator velocityCalculator;
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerOrder playerOrder;

        [field: Header("Debug"), SerializeField, ReadOnly]
        public bool ForwardCollision { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public bool AllowDownwardRaycast { get; private set; }

        public bool AllowForwardRaycast => allowForwardRaycast &&velocityCalculator.VelocityOrder > 0;

        private bool allowForwardRaycast;
        
        public void ResolveSameOrderCollision(RaycastHit[] hits)
        {
            int currentOrder = playerOrder.Order;
            
            //TODO: Filter same velocity collisions to prevent flickering
            FireUpwardCollisionRaycasts(hits);

            if (currentOrder != playerOrder.Order)
            {
                return;
            }
            
            playerOrder.IncrementOrder();
            StartCoroutine(TransitionDelay());
        }
        
        public void FireRayInMovementDirection(RaycastHit[] hits)
        {
            Transform playerCollisionTransform = transform;
            Vector3 raycastOrigin = raycastOriginForward;
            Vector3 raycastDirection = raycastDirectionForward;
            
            if (movement.MovementDirection == 1)
            {
                //right
                raycastOrigin = playerCollisionTransform.TransformPoint(raycastOrigin);
                raycastDirection.x = -raycastDirection.x;
                raycastDirection = playerCollisionTransform.TransformDirection(raycastDirection);
                
                ForwardCollision = FireRaycastIntoSameLayer(raycastOrigin, raycastDirection, hits) > 0;

#if UNITY_EDITOR
                Debug.DrawRay(raycastOrigin, raycastDirection, Color.magenta,1f);
#endif
                return;
            }
            
            //left
            raycastOrigin.x = -raycastOrigin.x;
            raycastOrigin = playerCollisionTransform.TransformPoint(raycastOrigin);
            raycastDirection = playerCollisionTransform.TransformDirection(raycastDirection);

            ForwardCollision = FireRaycastIntoSameLayer(raycastOrigin, raycastDirection, hits) > 0;

#if UNITY_EDITOR
            Debug.DrawRay(raycastOrigin, raycastDirection, Color.magenta,1f);
#endif
        }
        
        public void FireDownwardCollisionRaycasts(RaycastHit[] hits)
        {
            Transform playerCollisionTransform = transform;

            Vector3 raycastOriginLeft = raycastOriginDownward;
            raycastOriginLeft.x = -raycastOriginLeft.x;
            raycastOriginLeft = playerCollisionTransform.TransformPoint(raycastOriginLeft);
            
            Vector3 raycastDirection = playerCollisionTransform.TransformDirection(raycastDirectionDownward);

            int leftOrder = ComputeOrder(raycastOriginLeft, raycastDirection, hits, Color.red);

            Vector3 raycastOriginRight = playerCollisionTransform.TransformPoint(raycastOriginDownward);

            raycastDirection = raycastDirectionDownward;
            raycastDirection.x = -raycastDirection.x;
            raycastDirection = playerCollisionTransform.TransformDirection(raycastDirection);
                
            int rightOrder = ComputeOrder(raycastOriginRight, raycastDirection, hits, Color.blue);
        
            int newOrder = Mathf.Max(leftOrder, rightOrder) + 1;
            
            if (newOrder == playerOrder.Order) return;
            
            Vector3 raycastOriginMiddle = playerCollisionTransform.position;
            int middleOrder = ComputeOrder(raycastOriginMiddle, -transform.up, hits, Color.green);
            
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

        private int FireRaycastIntoSameLayer(Vector3 raycastOrigin, Vector3 raycastDirection, RaycastHit[] hits)
        {
            int mask = PlayerCollisionHelper.GetMaskForSameOrder(playerOrder.Order);
            
            // returns size of buffer as in number of hits
            return Physics.RaycastNonAlloc(raycastOrigin, raycastDirection, hits, raycastDirection.magnitude, mask);
        }
        
        private int FireRaycastIntoLayersBelow(Vector3 raycastOrigin, Vector3 raycastDirection, RaycastHit[] hits)
        {
            int mask = PlayerCollisionHelper.GetMaskForLowerOrders(playerOrder.Order);
            
            // returns size of buffer as in number of hits
            return Physics.RaycastNonAlloc(raycastOrigin, raycastDirection, hits, radius, mask);
        }
        
        private int FireRaycastIntoLayersAbove(Vector3 raycastOrigin, Vector3 raycastDirection, RaycastHit[] hits)
        {
            int mask = PlayerCollisionHelper.GetMaskForHigherOrders(playerOrder.Order);
            
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

        private int ComputeOrder(Vector3 raycastOrigin, Vector3 rayCastDirection, RaycastHit[] hits, Color debugColor, bool castBelow = true)
        {
            int order = -1;
            int rayCastHits = castBelow ? FireRaycastIntoLayersBelow(raycastOrigin, rayCastDirection, hits) 
                : FireRaycastIntoLayersAbove(raycastOrigin, rayCastDirection, hits);

        #if UNITY_EDITOR
        
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
            
        #else
            
            if(rayCastHits == 0) return order;

            order = GetOrderFromRaycastHits(hits, rayCastHits);
            
        #endif

            return order;
        }
        
        private void FireUpwardCollisionRaycasts(RaycastHit[] hits)
        {
            Transform playerCollisionTransform = transform;

            Vector3 raycastOriginLeft = raycastOriginDownward;
            raycastOriginLeft.x = -raycastOriginLeft.x;
            raycastOriginLeft = playerCollisionTransform.TransformPoint(raycastOriginLeft);

            Vector3 raycastDirection = transform.up;

            int leftOrder = ComputeOrder(raycastOriginLeft, raycastDirection, hits, Color.magenta, castBelow: false);

            Vector3 raycastOriginRight = playerCollisionTransform.TransformPoint(raycastOriginDownward);

            int rightOrder = ComputeOrder(raycastOriginRight, raycastDirection, hits, Color.cyan, castBelow: false);
        
            int newOrder = Mathf.Max(leftOrder, rightOrder) + 1;
            
            if (newOrder <= playerOrder.Order + 1) return;
            
            Vector3 raycastOriginMiddle = playerCollisionTransform.position;
            int middleOrder = ComputeOrder(raycastOriginMiddle, raycastDirection, hits, Color.green, castBelow: false);
            
            newOrder = Mathf.Max(newOrder, middleOrder + 1);
            
            if (newOrder <= playerOrder.Order + 1) return;

            playerOrder.OrderPlayer(newOrder);
            StartCoroutine(TransitionDelay());
        }

        private IEnumerator TransitionDelay()
        {
            AllowDownwardRaycast = false;
            ForwardCollision = false;
            allowForwardRaycast = false;
            
            yield return new WaitWhile(playerOrder.IsTransitionLocked);
            
            allowForwardRaycast = true;
            AllowDownwardRaycast = playerOrder.Order > 0;
        }
        
        private void Awake()
        {
            PlayerCollisionHelper.Register(playerCollider, this);
        }

        private void OnDisable()
        {
            allowForwardRaycast = true;
            AllowDownwardRaycast = false;
            ForwardCollision = false;
            StopAllCoroutines();
        }

        public int CompareTo(PlayerCollision other)
        {
            return playerOrder.Order.CompareTo(other.playerOrder.Order);
        }

#if UNITY_EDITOR

        [Space]
        [Header("Gizmos")]
        [SerializeField] private Color gizmoColorLeft;
        [SerializeField] private Color gizmoColorRight;

        private void OnDrawGizmosSelected()
        {
            var originTransform = transform;
            
            //down
            var origin = raycastOriginDownward;
            var originRight = originTransform.TransformPoint(origin);
            origin.x = -origin.x;
            var originLeft = originTransform.TransformPoint(origin);

            Gizmos.color = gizmoColorLeft;
            Gizmos.DrawSphere(originLeft, 0.03f);
            Gizmos.DrawLine(originLeft, originLeft + originTransform.TransformDirection(raycastDirectionDownward));

            Gizmos.color = gizmoColorRight;
            Gizmos.DrawSphere(originRight, 0.03f);

            Vector3 flipped = raycastDirectionDownward;
            flipped.x = -flipped.x;
            
            Gizmos.DrawLine(originRight, originRight + originTransform.TransformDirection(flipped));
            
            Gizmos.color = Color.green;
            flipped = transform.up * raycastDirectionDownward.magnitude;
            flipped = -flipped;
            Gizmos.DrawLine(originTransform.position, originTransform.position + flipped);

            //forward
            origin = raycastOriginForward;
            originRight = originTransform.TransformPoint(origin);
            origin.x = -origin.x;
            originLeft = originTransform.TransformPoint(origin);

            Gizmos.color = gizmoColorLeft;
            Gizmos.DrawSphere(originLeft, 0.03f);
            Gizmos.DrawLine(originLeft, originLeft + originTransform.TransformDirection(raycastDirectionForward));

            Gizmos.color = gizmoColorRight;
            Gizmos.DrawSphere(originRight, 0.03f);
            
            flipped = raycastDirectionForward;
            flipped.x = -flipped.x;
            Gizmos.DrawLine(originRight, originRight + originTransform.TransformDirection(flipped));
        }

#endif
    }
}