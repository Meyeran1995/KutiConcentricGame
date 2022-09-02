using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemMovement : MonoBehaviour
    {
        [field: SerializeField, Range(0f, 360f)] public float StartingAngle { get; set; }
        [field: SerializeField] public ItemMovementStatsSO Stats { get; private set; }

        #region Stats

        // Speed
        private float movementSpeed;

        // Rotation
        private bool useGlobalAxis, clockwiseRotation;
        private float rotationGain;
        private int maxAngle;
        
        // Switch stats
        private int switchValue, timesToSwitch;
        private float rotationGainModifier, speedModifier;
        private bool axisSwitch, rotationDirectionSwitch;

        #endregion
        
        private Vector3 movementDirection;
        private float currentAngle;
        private int stepCounter, switchCounter;
        private bool canSwitch;
        
#if UNITY_EDITOR

        private void Awake()
        {
            if(Stats == null) return;
            
            SetMovementStats(Stats);
            ResetFields();
        }

        private void Start()
        {
            if(TryGetComponent(out ItemMovementPrediction prediction))
                prediction.SetUpPrediction(this);
        }

#endif

        public void SetMovementStats(ItemMovementStatsSO stats)
        {
            Stats = stats;
            
            // Speed
            movementSpeed = stats.Speed;
            speedModifier = stats.SpeedModifier;

            // Rotation
            rotationGain = stats.RotationGain;
            rotationGainModifier = stats.RotationGainModifier;
            maxAngle = stats.MaxAngle;
            useGlobalAxis = stats.UseGlobalAxis;
            clockwiseRotation = stats.ClockwiseRotation;
            StartingAngle = stats.GetStartingAngle();

            // Switch stats
            switchValue = stats.Switch;
            timesToSwitch = stats.TimesToSwitch;
            axisSwitch = stats.AxisSwitch;
            rotationDirectionSwitch = stats.RotationDirectionSwitch;
            
            ResetFields();
        }

        public void ResetFields()
        {
            movementDirection = GetStartDirection(transform.position).normalized;
            currentAngle = 0f;
            canSwitch = switchValue != 0;
            stepCounter = 0;
            switchCounter = 0;
        }

        private void FixedUpdate()
        {
            currentAngle += rotationGain * Time.fixedDeltaTime;

            if ((int)currentAngle > maxAngle)
            {
                currentAngle = maxAngle;
            }
            
            Move();
            
            if ((int)currentAngle == maxAngle)
            {
                currentAngle = 0f;
            }
            
            if(!canSwitch) return;
            
            EvaluateSwitch();
        }

        private void Move()
        {
            Vector3 currentPosition = transform.position;
            var currentRotation = GetRotation(currentPosition, currentAngle, useGlobalAxis, clockwiseRotation);
            transform.position = GetNextPosition(currentPosition, movementDirection, currentRotation, movementSpeed);
        }

        private void EvaluateSwitch()
        {
            stepCounter++;
            
            if(stepCounter % switchValue != 0) return;

            switchCounter++;
            
            ApplySwitchModifiers();
            if (switchCounter == timesToSwitch)
                canSwitch = false;
            
            stepCounter = 0;
        }

        private void ApplySwitchModifiers()
        {
            rotationGain += rotationGainModifier;
            movementSpeed += speedModifier;

            if (axisSwitch)
                useGlobalAxis = !useGlobalAxis;

            if (rotationDirectionSwitch)
                clockwiseRotation = !clockwiseRotation;
        }

        public Quaternion GetRotation(Vector3 currentPosition, float angle, bool globalAxisUsage, bool clockWiseRotation)
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

        public Vector3 GetNextPosition(Vector3 currentPosition, Vector3 direction, Quaternion currentRotation, float speed)
        {
            Vector3 nextPosition = currentRotation * direction;
            nextPosition *= speed * Time.fixedDeltaTime;
            nextPosition += currentPosition;
            nextPosition.z = 0f;

            return nextPosition;
        }
        
        public Vector3 GetStartDirection(Vector2 startingPosition)
        {
            var currentPosition = startingPosition;
            float angleInRad = Mathf.Deg2Rad * StartingAngle;
            float x = Mathf.Cos(angleInRad);
            float y = Mathf.Sin(angleInRad);

            return new Vector3(x - currentPosition.x, y - currentPosition.y);
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            if(TryGetComponent(out ItemMovementPrediction prediction))
                prediction.SetUpPrediction(this);
        }
        
#endif
        
    }
}
