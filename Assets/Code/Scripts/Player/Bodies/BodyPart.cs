using System.Collections;
using Meyham.Collision;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.U2D;

namespace Meyham.Player.Bodies
{
    public class BodyPart : MonoBehaviour
    {
        private const float order_displacement_amount = 32f / 100f;
        
        [Header("Model")]
        [SerializeField] private SpriteShapeRenderer spriteRenderer;
        [SerializeField] private BodyTweenAnimation tweenAnimation;
        
        [Header("Body")]
        [SerializeField] private Collider bodyCollider;
        [SerializeField] private Collider itemCollider;

        [Space, SerializeField] private OrderTween orderTween;
        
        [field: Header("Debug"), SerializeField, ReadOnly]
        public int Order { get; private set; }

        [SerializeField, ReadOnly] private bool transitionLocked;
        
        public void SetColor(Color activeColor)
        {
            spriteRenderer.color = activeColor;
        }

        public BodyTweenAnimation GetTweenAnimation()
        {
            return tweenAnimation;
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
        
        public void UpdatePlayerOrder(int newOrder)
        {
            if(transitionLocked) return;

            Order = newOrder;
            OrderPlayerAsync();
        }

        private void OnEnable()
        {
            if(Order == 0) return;
            
            Order = 0;
            orderTween.transform.localPosition = Vector3.zero;
            bodyCollider.gameObject.layer = PlayerCollisionHelper.GetLayer(0);
        }

        private void OnDisable()
        {
            transitionLocked = false;
        }
        
        private void OrderPlayerAsync()
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

        private IEnumerator OrderTransition(Vector3 localPosition)
        {
            transitionLocked = true;
            yield return orderTween.TweenToPosition(localPosition, Order);
            transitionLocked = false;
        }
    }
}
