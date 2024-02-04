using System.Collections;
using Meyham.Collision;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.U2D;

namespace Meyham.Player.Bodies
{
    public class BodyPart : MonoBehaviour
    {
        [Header("Model")]
        [SerializeField] private SpriteShapeController spriteShapeController;
        [SerializeField] private SpriteShapeRenderer spriteRenderer;
        
        [Header("Body")]
        [SerializeField] private Collider bodyCollider;
        [SerializeField] private Collider itemCollider;

        [Space, SerializeField] private OrderTween tween;
        
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
            bodyCollider.enabled = true;
            itemCollider.enabled = true;
        }
        
        public void Hide()
        {
            spriteRenderer.enabled = false;
            bodyCollider.enabled = false;
            itemCollider.enabled = false;
        }

        public void IgnoreRaycast()
        {
            bodyCollider.gameObject.layer = 2;
        }

        public void AllowRaycast()
        {
            bodyCollider.gameObject.layer = PlayerCollisionHelper.GetLayer(Order);
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

            if (Order == 0)
            {
                StartCoroutine(OrderTransition(Vector3.zero));
                bodyCollider.gameObject.layer = PlayerCollisionHelper.GetLayer(0); 
                return;
            }

            var displacement = new Vector3(0f, Order * order_displacement_amount, 0f);
            
            StartCoroutine(OrderTransition(displacement));
            bodyCollider.gameObject.layer = PlayerCollisionHelper.GetLayer(Order);
        }
        
        private void Start()
        {
            spline = spriteShapeController.spline;
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

        private IEnumerator OrderTransition(Vector3 localPosition)
        {
            transitionLocked = true;
            yield return tween.TweenToPosition(localPosition);
            transitionLocked = false;
        }
    }
}
