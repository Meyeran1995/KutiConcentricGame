using System.Collections;
using Meyham.DataObjects;
using Meyham.GameMode;
using Meyham.Splines;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemCollision : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Collider itemCollider;
        [Space]
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private ItemTweeningAnimation tweenAnimation;
        [Space] 
        [SerializeField] private ItemCollectibleCarrier collectibleCarrier;

        private static CollectiblePool pool;

        public void ReceiveColliderDimensions(Vector3 localPosition, Quaternion localRotation, 
            Vector3 localScale)
        {
            var collisionTransform = transform;
            collisionTransform.localPosition = localPosition;
            collisionTransform.localRotation = localRotation;
            collisionTransform.localScale = localScale;
        }
        
        private void Awake()
        {
            pool ??= GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectiblePool>();
        }
        
        private void OnEnable()
        {
            itemCollider.isTrigger = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(!collectibleCarrier.enabled) return;
            
            var incomingObject = other.attachedRigidbody.gameObject;

            if (!incomingObject.CompareTag("Player")) return;

            itemCollider.isTrigger = false;
            splineFollower.Pause();
            
            collectibleCarrier.OnCollected(incomingObject);
            
            StartCoroutine(WaitForShrinkAnimation());
        }

        private IEnumerator WaitForShrinkAnimation()
        {
            yield return tweenAnimation.TweenShrink();
            
            pool.ReleaseCollectible(transform.parent.gameObject);
        }
    }
}