﻿using System.Collections;
using Meyham.Collision;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerOrder : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private FloatParameter sizeFactor;
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody playerRigidbody;
        [Header("Player references")]
        [SerializeField] private PlayerModelProvider modelProvider;
        [SerializeField] private PlayerColliderUpdater colliderUpdater;
        [SerializeField] private PlayerCollision playerCollision;

        [field: Header("Debug"), SerializeField, ReadOnly]
        public int Order { get; private set; }

        [SerializeField, ReadOnly] private bool transitionLocked;
        
        private bool wasModified;

        private const float order_displacement_amount = 32f / 100f;

        public bool IsTransitionLocked()
        {
            return transitionLocked;
        }
        
        public void IncrementOrder()
        {
            Order++;
            wasModified = true;
            
            if (Order < 5) return;
            
            Order = 5;
        }

        public void OrderPlayer(int order)
        {
            Order = order;
            wasModified = true;
        }

        public void UpdatePlayerOrder()
        {
            if(!wasModified || transitionLocked) return;
            OrderPlayer();
            wasModified = false;
        }
        
        private void OrderPlayer()
        {
            StartCoroutine(OrderTransition());
            spriteRenderer.sprite = modelProvider.GetModel(Order);
            var modelTransform = transform;

            if (Order == 0)
            {
                modelTransform.localPosition = Vector3.zero;
                playerRigidbody.transform.position = modelTransform.position;
                colliderUpdater.ModifyCollisionSize(1f, 0);
                playerCollision.OnOrderChanged(0);
               return;
            }

            var displacement = new Vector3(-Order * order_displacement_amount, 0f, 0f);
            modelTransform.localPosition = displacement;
            
            playerRigidbody.transform.position = modelTransform.position;
            colliderUpdater.ModifyCollisionSize(sizeFactor.RuntimeValue, Order);
            playerCollision.OnOrderChanged(Order);
        }

        private void OnEnable()
        {
            if(Order == 0) return;
            
            Order = 0;
            OrderPlayer();
        }

        private void OnDisable()
        {
            transitionLocked = false;
            StopAllCoroutines();
        }

        private IEnumerator OrderTransition()
        {
            transitionLocked = true;
            yield return new WaitForFixedUpdate();
            transitionLocked = false;
        }
    }
}