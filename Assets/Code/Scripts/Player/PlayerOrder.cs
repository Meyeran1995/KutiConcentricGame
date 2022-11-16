using Meyham.Collision;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerOrder : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private FloatValue sizeFactor;
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody playerRigidbody;
        [Header("Player references")]
        [SerializeField] private PlayerModelProvider modelProvider;
        [SerializeField] private PlayerCollisionHelper collisionHelper;
        [SerializeField] private PlayerVelocityCalculator velocityCalculator;
        
        private const float OrderDisplacementAmount = 32f / 100f;

        [field: SerializeField, ReadOnly]
        public int Order { get; private set; }

        public float PlayerVelocity => velocityCalculator.LastVelocity;
        
        private bool wasModified;

        public void IncrementOrder()
        {
            ++Order;
            wasModified = true;
            
            if (Order < 5) return;
            
            Order = 5;
        }

        public void DecrementOrder()
        {
            --Order;
            wasModified = true;

            if (Order >= 0) return;
            
            Order = 0;
        }

        public void OrderPlayer(int order)
        {
            Order = order;
            wasModified = true;
        }
        
        private void OrderPlayer()
        {
            spriteRenderer.sprite = modelProvider.GetModel(Order);
            var modelTransform = transform;
            
            if (Order == 0)
            {
                modelTransform.localPosition = Vector3.zero;
                
                playerRigidbody.MovePosition(modelTransform.position);
                collisionHelper.ModifyCollisionSize(1f, 0);
                return;
            }

            var displacement = new Vector3(-Order * OrderDisplacementAmount, 0f, 0f);
            modelTransform.localPosition = displacement;
            
            playerRigidbody.MovePosition(modelTransform.position);
            collisionHelper.ModifyCollisionSize(sizeFactor.RuntimeValue, Order);
        }

        private void OnEnable()
        {
            if(Order == 0) return;
            OrderPlayer(0);
        }

        private void FixedUpdate()
        {
            if(!wasModified) return;
            
            OrderPlayer();
            wasModified = false;
        }
    }
}