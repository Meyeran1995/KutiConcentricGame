using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerCollisionHelper : MonoBehaviour
    {
        private static Transform spawnTransform;
        private static Vector3 startingScale;

        public void FaceSpawn()
        {
            var direction = spawnTransform.position - transform.position;
            direction.Normalize();
            
            var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        public void ModifyCollisionSize(float sizeFactor, int order)
        {
            if (order == 0)
            {
                transform.localScale = startingScale;
                return;
            }

            float scalePercentage = 1f - sizeFactor * order;
            Vector3 newScale = startingScale;
            newScale.x *= scalePercentage;
            
            transform.localScale = newScale;
        }

        private void Awake()
        {
            if (!spawnTransform)
                spawnTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
            
            if(startingScale != Vector3.zero) return;
            
            startingScale = transform.localScale;
        }
    }
}
