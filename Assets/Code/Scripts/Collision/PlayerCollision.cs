﻿using System;
using Meyham.DataObjects;
using Meyham.Player;
using Meyham.Player.Bodies;
using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollision : MonoBehaviour, IComparable<PlayerCollision>
    {
        [SerializeField] private FloatParameter raycastLength;
        [SerializeField] private FloatParameter radius;
        
        [Header("Raycast Origins")]
        [SerializeField] private Vector2Parameter raycastOriginForward;
        [SerializeField] private Vector2Parameter raycastOriginDownward;
        
        [Header("Raycast Direction")]
        [SerializeField] private Vector2Parameter raycastDirectionForward;
        [SerializeField] private Vector2Parameter raycastDirectionDownward;
        
        [Header("References")]
        [SerializeField] private PlayerVelocityCalculator velocityCalculator;
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerBody playerBody;

        private RaycastData counterClockwise, clockwise, downLeft, downRight;
        
        public int CompareTo(PlayerCollision other)
        {
            // compares based on velocityorder and bodycount
            var compareA = velocityCalculator.VelocityOrder - playerBody.Count;
            var compareB = other.velocityCalculator.VelocityOrder - other.playerBody.Count;
            
            return compareB.CompareTo(compareA);
        }

        public void OnStartCollisionChecks()
        {
            playerBody.IgnoreRaycast();
        }

        public void OnFinishedCollisionChecks()
        {
            playerBody.AllowRaycast();
        }
        
        public void ResolveForwardCollisions(RaycastHit[] hits)
        {
            if (playerBody.Count == 0) return;
            
            var bodyParts = playerBody.GetBodyParts();
            var bodyPart = bodyParts[0];
            var previousOrder = 5;

            if (bodyPart.Order != 5 && !bodyPart.IsTransitionLocked())
            {
                previousOrder = bodyPart.Order;
                
                ResolveSameOrderCollision(bodyPart, hits);
            }
            
            for (var i = 1; i < bodyParts.Length; i++)
            {
                bodyPart = bodyParts[i];

                var currentOrder = bodyPart.Order;

                if (currentOrder == 5 || currentOrder == previousOrder || bodyPart.IsTransitionLocked())
                {
                    previousOrder = currentOrder;
                    continue;
                }

                ResolveSameOrderCollision(bodyPart, hits);
                previousOrder = currentOrder;
            }
        }

        public void ResolveDownwardChecks(RaycastHit[] hits)
        {
            foreach (var bodyPart in playerBody.GetBodyParts())
            {
                if(bodyPart.Order == 0 || bodyPart.IsTransitionLocked()) continue;
                
                FireDownwardCollisionRaycasts(bodyPart, hits);
            }
        }
        
        private void ResolveSameOrderCollision(BodyPart bodyPart, RaycastHit[] hits)
        {
            var bodyCollision = PlayerCollisionHelper.GetCollisionByBodyPart(bodyPart);
            var rayCastData = movement.MovementDirection == -1 ? clockwise : counterClockwise;
            var forwardHits = bodyCollision.FireRayInMovementDirection(rayCastData, hits);
            
            if(forwardHits == 0) return;
            
            bodyPart.UpdatePlayerOrder(GetOrderFromRaycastHits(forwardHits, hits) + 1);
        }

        private void FireDownwardCollisionRaycasts(BodyPart bodyPart, RaycastHit[] hits)
        {
            var bodyCollision = PlayerCollisionHelper.GetCollisionByBodyPart(bodyPart);
            var numberOfHits = bodyCollision.FireDownWardRaycast(downLeft, hits);

            var leftOrder = -1;
            if (numberOfHits > 0)
            {
                leftOrder = GetOrderFromRaycastHits(numberOfHits, hits);
            }

            numberOfHits = bodyCollision.FireDownWardRaycast(downRight, hits);
            
            var rightOrder = -1;
            if (numberOfHits > 0)
            {
                rightOrder = GetOrderFromRaycastHits(numberOfHits, hits);
            }
            
            var newOrder = Mathf.Max(leftOrder, rightOrder) + 1;
            
            if (newOrder >= bodyPart.Order) return;

            numberOfHits = bodyCollision.FireDownwardCenterRaycast(radius, hits);
            
            if (numberOfHits == 0)
            {
                bodyPart.UpdatePlayerOrder(newOrder);
                return;
            }
            
            var middleOrder = GetOrderFromRaycastHits(numberOfHits, hits) + 1;
            newOrder = Mathf.Max(newOrder, middleOrder);
            
            if (newOrder >= bodyPart.Order) return;
            
            bodyPart.UpdatePlayerOrder(newOrder);
        }

        private int GetOrderFromRaycastHits(int numberOfHits, RaycastHit[] hits)
        {
            int order = -1;

            for (int i = 0; i < numberOfHits; i++)
            {
                var body = hits[i].collider.GetComponentInParent<BodyPart>();
                int hitOrder = body.Order;

                if (hitOrder <= order) continue;

                order = hitOrder;
            }

            return order;
        }

        private void Start()
        {
            BuildRayCastData();
        }
        
        private void BuildRayCastData()
        {
            //left movement == -1
            Vector3 raycastOrigin = raycastOriginForward;
            raycastOrigin.x = -raycastOrigin.x;
            
            clockwise = new RaycastData(raycastOrigin, raycastDirectionForward, raycastLength);
            
            //right movement == 1
            Vector3 raycastDirection = raycastDirectionForward;
            raycastDirection.x = -raycastDirection.x;

            counterClockwise = new RaycastData(raycastOriginForward, raycastDirection, raycastLength);

            //downward
            raycastDirection = raycastDirectionDownward;
            raycastDirection.x = -raycastDirection.x;
            
            downRight = new RaycastData(raycastOriginDownward, raycastDirection, radius);

            raycastOrigin = raycastOriginDownward;
            raycastOrigin.x = -raycastOrigin.x;
            
            downLeft = new RaycastData(raycastOrigin, raycastDirectionDownward, radius);
        }

#if UNITY_EDITOR

        [Space]
        [Header("Gizmos")]
        [SerializeField] private Color gizmoColorLeft;
        [SerializeField] private Color gizmoColorRight;
        [SerializeField] private Transform originTransform;
        [SerializeField] private bool drawGizmos;

        private float lastSumOfMagnitudes;

        private void Update()
        {
            var current = raycastDirectionDownward.RuntimeValue.magnitude;
            current += raycastDirectionForward.RuntimeValue.magnitude;
            current += raycastOriginForward.RuntimeValue.magnitude;
            current += raycastOriginDownward.RuntimeValue.magnitude;
            current += raycastLength;

            if (Mathf.Abs(lastSumOfMagnitudes - current) < 0.01f)
            {
                lastSumOfMagnitudes = current;
                return;
            }
            
            BuildRayCastData();
            
            lastSumOfMagnitudes = current;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            
            //down
            var origin = raycastOriginDownward.RuntimeValue;
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
            flipped = originTransform.up * flipped.magnitude;
            flipped = -flipped;
            Gizmos.DrawLine(originTransform.position, originTransform.position + flipped);

            //forward
            origin = raycastOriginForward;
            originRight = originTransform.TransformPoint(origin);
            origin.x = -origin.x;
            originLeft = originTransform.TransformPoint(origin);

            Vector3 direction = raycastDirectionForward;
            direction.Normalize();
            direction *= raycastLength;
            
            Gizmos.color = gizmoColorLeft;
            Gizmos.DrawSphere(originLeft, 0.03f);
            Gizmos.DrawLine(originLeft, originLeft + originTransform.TransformDirection(direction));

            Gizmos.color = gizmoColorRight;
            Gizmos.DrawSphere(originRight, 0.03f);
            
            flipped = direction;
            flipped.x = -flipped.x;
            Gizmos.DrawLine(originRight, originRight + originTransform.TransformDirection(flipped));
        }

#endif
    }
}