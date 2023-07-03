using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.Items
{
    public class ItemRotator : MonoBehaviour
    {
        [SerializeField] private FloatParameter maxRotationSpeed;

        [SerializeField, ReadOnly] private float rotationSpeed;
        
        private void OnEnable()
        {
            rotationSpeed = Random.Range(maxRotationSpeed / 4f, maxRotationSpeed) * RandomSign();
        }

        private void Update()
        {
            transform.Rotate(transform.forward, rotationSpeed * Time.deltaTime);
        }

        private static int RandomSign()
        {
            if (Random.value < 0.5f)
            {
                return -1;
            }

            return 1;
        }
    }
}
