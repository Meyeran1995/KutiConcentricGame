using UnityEngine;

namespace Meyham.Items
{
    public class ItemMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [Header("Size")]
        [SerializeField] private float sizeGain;
        [SerializeField] private float maxSize;
        [Header("Rotation")]
        [SerializeField] private bool useGlobalAxis;
        [SerializeField] private bool clockWiseRotation;
        [SerializeField, Range(0f, 360f)] private float rotationGain, startingAngle;
        [SerializeField, Range(0, 360)] private int maxAngle;
        [SerializeField, Min(0)] private int axisSwitch;

        private Vector3 movementDirection, sizeIncrease;
        private float currentAngle;
        private int stepCounter;
        private bool maxSizeReached;

        private void Awake()
        {
            movementDirection = GetStartDirection().normalized;
            sizeIncrease = new Vector3(sizeGain, sizeGain);
            currentAngle = rotationGain * Time.deltaTime;
        }

        private void FixedUpdate()
        {
            currentAngle += rotationGain * Time.fixedDeltaTime;

            if (!maxSizeReached)
            {
                transform.localScale += sizeIncrease * Time.fixedDeltaTime;
                maxSizeReached = transform.localScale.x >= maxSize;

                if (maxSizeReached)
                    transform.localScale = new Vector3(maxSize, maxSize, maxSize);
            }
            
            Move();
        }

        private void Move()
        {
            Vector3 currentPosition = transform.position;
            var currentRotation = GetRotation(currentPosition, currentAngle, useGlobalAxis);
            transform.position = GetNextPosition(currentPosition, movementDirection, currentRotation);
            
            stepCounter++;
            
            if ((int)currentAngle == maxAngle)
            {
                currentAngle = rotationGain * Time.deltaTime;
            }

            if(axisSwitch == 0) return;
            if(stepCounter % axisSwitch != 0) return;

            useGlobalAxis = !useGlobalAxis;
            stepCounter = 0;
        }

        private Quaternion GetRotation(Vector3 currentPosition, float angle, bool globalAxisUsage)
        {
            Quaternion currentRotation;
            float rotationAngle = clockWiseRotation ? -angle : angle;

            if (globalAxisUsage)
            {
                currentRotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
            }
            else
            {
                var currentRotationAxis = currentPosition;
                currentRotationAxis.z = 1f;
                currentRotation = Quaternion.AngleAxis(rotationAngle, currentRotationAxis);
            }

            return currentRotation;
        }
        
        private Vector3 GetNextPosition(Vector3 currentPosition, Vector3 direction, Quaternion currentRotation)
        {
            Vector3 nextPosition = currentRotation * direction;
            nextPosition *= speed * Time.fixedDeltaTime;
            nextPosition += currentPosition;
            nextPosition.z += sizeGain * Time.fixedDeltaTime;

            return nextPosition;
        }
        
        private Vector3 GetStartDirection()
        {
            var currentPosition = transform.position;
            float angleInRad = Mathf.Deg2Rad * startingAngle;
            float x = Mathf.Cos(angleInRad);
            float y = Mathf.Sin(angleInRad);

            return new Vector3(x - currentPosition.x, y - currentPosition.y);
        }
        
#if UNITY_EDITOR

        [Header("Gizmos")]
        [SerializeField, Min(0)] private int predictionSteps;
        [SerializeField, Min(0)] private int resolution;
        [SerializeField, Min(0f)] private float gizmoSphereRadius;

        private const int stepsToSecond = 50;
        
        private void OnDrawGizmosSelected()
        {
            if(predictionSteps == 0 || resolution == 0 || gizmoSphereRadius == 0f) return;
            
            Vector3 direction = GetStartDirection().normalized;
            Vector3 currentPosition = transform.position;
            float currentSize = transform.localScale.x;
            float angle = rotationGain * Time.fixedDeltaTime;
            
            bool globalAxis = useGlobalAxis;

            for (int i = 0; i <= predictionSteps; i++)
            {
                angle += rotationGain * Time.fixedDeltaTime;
                
                if(currentSize < maxSize)
                    currentSize += sizeGain * Time.fixedDeltaTime;
                
                currentPosition = GetNextPosition(currentPosition, direction, GetRotation(currentPosition, angle, globalAxis));

                if ((int)angle == maxAngle)
                {
                    angle = rotationGain * Time.fixedDeltaTime;
                }
                
                if (axisSwitch != 0 && i % axisSwitch == 0)
                    globalAxis = !globalAxis;

                if (i % stepsToSecond == 0)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(currentPosition, currentSize * gizmoSphereRadius);
                    continue;
                }
                
                if(i % resolution != 0) continue;
                
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(currentPosition, currentSize * gizmoSphereRadius);
            }
        }
        
#endif
    }
}
