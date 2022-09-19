using UnityEngine;

namespace Meyham.Player
{
    public class PlayerCollisionHelper : MonoBehaviour
    {
        private static Transform spawnTransform;

        private void Start()
        {
            if (!spawnTransform)
                spawnTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
        }

        private void FixedUpdate()
        {
            FaceCamera();
        }

        private void FaceCamera()
        {
            transform.LookAt(spawnTransform.position);
        }
    }
}
