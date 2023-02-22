using UnityEngine;

namespace Meyham.Items
{
    public class ItemCollisionUpdater : MonoBehaviour
    {
        private static Transform cameraTransform;

        private void Awake()
        { 
            cameraTransform ??= Camera.main.transform;
        }

        private void FixedUpdate()
        {
            FaceCamera();
        }

        private void FaceCamera()
        {
            transform.LookAt(cameraTransform.position);
        }
    }
}