using Meyham.Player.Bodies;
using UnityEngine;

namespace Meyham.Collision
{
    public class BodyCollision : MonoBehaviour
    {
        [SerializeField] private BodyPart bodyPart;
        
        public bool FireRayInMovementDirection(RaycastData raycastData, RaycastHit[] hits)
        {
            var transformSelf = transform;
            var raycastOrigin = transformSelf.TransformPoint(raycastData.Origin);
            var raycastDirection = transformSelf.TransformDirection(raycastData.Direction);

            var mask = PlayerCollisionHelper.GetMaskForSameOrder(bodyPart.Order);
            
#if UNITY_EDITOR
            Debug.DrawRay(raycastOrigin, raycastDirection.normalized * raycastData.Length, Color.magenta,1f);
#endif
            
            // returns size of buffer as in number of hits
            return Physics.RaycastNonAlloc(raycastOrigin, raycastDirection, hits, raycastData.Length, mask) > 0;
        }
        
        public int FireDownWardRaycast(RaycastData raycastData, RaycastHit[] hits)
        {
            var transformSelf = transform;
            var raycastOrigin = transformSelf.TransformPoint(raycastData.Origin);
            var raycastDirection = transformSelf.TransformDirection(raycastData.Direction);
            
            var mask = PlayerCollisionHelper.GetMaskForLowerOrders(bodyPart.Order);
            
#if UNITY_EDITOR
            Debug.DrawRay(raycastOrigin, raycastDirection.normalized * raycastData.Length, Color.blue,1f);
#endif
            
            return Physics.RaycastNonAlloc(raycastOrigin, raycastDirection, hits, raycastData.Length, mask);
        }

        public int FireDownwardCenterRaycast(float raycastLength, RaycastHit[] hits)
        {
            var transformSelf = transform;
            var mask = PlayerCollisionHelper.GetMaskForLowerOrders(bodyPart.Order);
            
#if UNITY_EDITOR
            Debug.DrawRay(transformSelf.position, -transformSelf.up * raycastLength, Color.blue,1f);
#endif
            
            return Physics.RaycastNonAlloc(transformSelf.position, -transformSelf.up, hits, raycastLength, mask);
        }

        public int FireUpwardRaycast(RaycastData raycastData, RaycastHit[] hits)
        {
            Transform transformSelf = transform;

            var raycastOrigin = transformSelf.TransformPoint(raycastData.Origin);
            var raycastDirection = transformSelf.up;
            
            int mask = PlayerCollisionHelper.GetMaskForHigherOrders(bodyPart.Order);
            
#if UNITY_EDITOR
            Debug.DrawRay(raycastOrigin, raycastDirection.normalized * raycastData.Length, Color.red,1f);
#endif
            
            return Physics.RaycastNonAlloc(raycastOrigin, raycastDirection, hits, raycastData.Length, mask);
        }
        
        public int FireUpwardCenterRaycast(float raycastLength, RaycastHit[] hits)
        {
            var transformSelf = transform;
            var mask = PlayerCollisionHelper.GetMaskForHigherOrders(bodyPart.Order);
            
#if UNITY_EDITOR
            Debug.DrawRay(transformSelf.position, -transformSelf.up * raycastLength, Color.blue,1f);
#endif
            
            return Physics.RaycastNonAlloc(transformSelf.position, transformSelf.up, hits, raycastLength, mask);
        }

        private void Awake()
        {
            PlayerCollisionHelper.Register(bodyPart, this);
        }

        private void OnDestroy()
        {
            PlayerCollisionHelper.DeRegister(bodyPart);
        }
    }
}
