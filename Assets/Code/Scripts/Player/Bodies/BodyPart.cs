using System.Collections;
using Meyham.Collision;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.U2D;

namespace Meyham.Player.Bodies
{
    public class BodyPart : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController body;
        [SerializeField] private SpriteShapeRenderer spriteRenderer;
        [SerializeField] private Collider bodyCollider;
        
        [field: Header("Debug"), SerializeField, ReadOnly]
        public int Order { get; private set; }

        [SerializeField, ReadOnly] private bool transitionLocked;
        
        private bool wasModified;

        private const float order_displacement_amount = 32f / 100f;
        
        private Spline spline;
        
        //collisionen checken
            // vorwärts -> kann man skippen, wenn nicht kopf
            // unten -> kann man skippen wenn order == 0
        //hat eigene order
            // muss basierend auf der order lokale position der spline punkte animieren
        //kennt base settings für reset/pooling

        public void SetColor(Color activeColor)
        {
            spriteRenderer.color = activeColor;
        }
        
        public void Show()
        {
            spriteRenderer.enabled = true;
        }
        
        public void Hide()
        {
            spriteRenderer.enabled = false;
        }

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
            var modelTransform = spriteRenderer.transform;
            var collisionTransform = bodyCollider.transform;

            if (Order == 0)
            {
                modelTransform.localPosition = Vector3.zero;
                collisionTransform.localPosition = Vector3.zero;
                bodyCollider.gameObject.layer = PlayerCollisionHelper.GetLayer(0);
               return;
            }

            var displacement = new Vector3(0f, Order * order_displacement_amount, 0f);
            modelTransform.localPosition = displacement;
            collisionTransform.localPosition = displacement;

            bodyCollider.gameObject.layer = PlayerCollisionHelper.GetLayer(Order);
        }
        
        private void Start()
        {
            spline = body.spline;
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
        }

        private IEnumerator OrderTransition()
        {
            transitionLocked = true;
            yield return new WaitForFixedUpdate();
            transitionLocked = false;
        }
    }
}