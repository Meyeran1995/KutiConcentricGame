using UnityEngine;

namespace Meyham.Items
{
    public class ItemCollision : MonoBehaviour
    {
        private static Transform cameraTransform;

        private void Start()
        {
            if (!cameraTransform)
                cameraTransform = Camera.main.transform;
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