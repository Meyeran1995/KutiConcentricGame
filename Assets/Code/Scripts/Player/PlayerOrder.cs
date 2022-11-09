using System;
using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerOrder : MonoBehaviour
    {
        [SerializeField] private FloatValue sizeFactor;
        [SerializeField] private PlayerModelProvider modelProvider;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform playerModel;
        [SerializeField] private Collider playerCollider;
        [SerializeField] private PlayerCollisionHelper collisionHelper;

        private const float OrderDisplacementAmount = 32f / 100f;

        [field: SerializeField, ReadOnly]
        public int Order { get; private set; }

        private bool wasModified;
        private List<Collider> collisions;

        public void IncrementOrder()
        {
            ++Order;
            
            if (Order > 5)
            {
                Order = 5;
            }

            wasModified = true;
        }

        public void DecrementOrder()
        {
            --Order;
            
            if (Order < 0)
            {
                Order = 0;
            }

            wasModified = true;
        }

        public void OrderPlayer(int order)
        {
            Order = order;
            spriteRenderer.sprite = modelProvider.GetModel(order);
            playerModel.localPosition = new Vector3(-order * OrderDisplacementAmount, 0f, 0f);
            collisionHelper.ModifyCollisionSize(sizeFactor.RuntimeValue, order);            
        }
        
        private void OrderPlayer()
        {
            spriteRenderer.sprite = modelProvider.GetModel(Order);
            playerModel.localPosition = new Vector3(-Order * OrderDisplacementAmount, 0f, 0f);
            collisionHelper.ModifyCollisionSize(sizeFactor.RuntimeValue, Order);            
        }
      
        private void Awake()
        {
            collisions = new List<Collider>(5);
        }

        private void OnEnable()
        {
            if(Order == 0) return;
            OrderPlayer(0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(collisions.Contains(other)) return;

            var otherOrder = other.GetComponent<PlayerOrder>();
            
            if(otherOrder.Order < Order) return;
            
            otherOrder.IncomingCollision(playerCollider);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!collisions.Contains(other)) return;
            collisions.Remove(other);
            PlayerCollisionResolver.AddExit(this, other);
        }
        
        private void IncomingCollision(Collider other)
        {
            collisions.Add(other);
            PlayerCollisionResolver.AddCollision(this, other);
        }

        private void LateUpdate()
        {
            if(!wasModified) return;
            
            OrderPlayer();
            wasModified = false;
        }
    }
}