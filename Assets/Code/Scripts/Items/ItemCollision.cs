using System.Collections;
using Meyham.GameMode;
using Meyham.Splines;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemCollision : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Collider itemCollider;
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private ItemCollectibleCarrier collectibleCarrier;
        
        public void ReceiveColliderDimensions(Vector3 localPosition, Quaternion localRotation, 
            Vector3 localScale)
        {
            var collisionTransform = transform;
            collisionTransform.localPosition = localPosition;
            collisionTransform.localRotation = localRotation;
            collisionTransform.localScale = localScale;
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
        }
    }
}