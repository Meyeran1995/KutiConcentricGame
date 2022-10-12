using UnityEngine;

namespace Meyham.Player
{
    public class PlayerCollisionHelper : MonoBehaviour
    {
        private static Transform spawnTransform;

        public void FaceSpawn()
        {
            var collisionTransform = transform;
            var direction = spawnTransform.position - collisionTransform.position;
            direction.Normalize();
            
            var angle = Vector3.SignedAngle(collisionTransform.forward, direction, Vector3.up);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
        
        private void Awake()
        {
            if (!spawnTransform)
                spawnTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
        }
    }
}
