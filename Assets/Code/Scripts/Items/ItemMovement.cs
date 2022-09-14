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
        private float sidewaysSpeed, forwardSpeed;

        // Rotation
        private bool useGlobalAxis, clockwiseRotation;
        private float rotationGain;
        private int maxAngle;
        
        // Switch stats
        private int switchValue, timesToSwitch;
        private float rotationGainModifier, speedModifier;
        private bool axisSwitch, rotationDirectionSwitch;

        #endregion
        
        private Vector2 movementDirection;
        private float currentAngle;
        private int stepCounter, switchCounter;
        private bool canSwitch;
        
#if UNITY_EDITOR

        private void Awake()
        {
            if(Stats == null) return;
            
            SetMovementStats(Stats);
        }

#endif

        public void SetMovementStats(ItemMovementStatsSO stats)
        {
            Stats = stats;
            
            // Speed
            sidewaysSpeed = stats.SidewaysSpeed * Time.fixedDeltaTime;
            forwardSpeed = stats.ForwardSpeed * Time.fixedDeltaTime;
            speedModifier = stats.SidewaysSpeedModifier * Time.fixedDeltaTime;

            // Rotation
            rotationGain = stats.RotationGain * Time.fixedDeltaTime;
            rotationGainModifier = stats.RotationGainModifier * Time.fixedDeltaTime;
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

        private void FixedUpdate()
        {
            currentAngle += rotationGain;

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
            transform.position = GetNextPosition();
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
            sidewaysSpeed += speedModifier;

            if (axisSwitch)
                useGlobalAxis = !useGlobalAxis;

            if (rotationDirectionSwitch)
                clockwiseRotation = !clockwiseRotation;
        }

        private Quaternion GetRotation()
        {
            Quaternion currentRotation;
            float rotationAngle = clockwiseRotation ? -currentAngle : currentAngle;

            if (useGlobalAxis)
            {
                currentRotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
            }
            else
            {
                var currentRotationAxis = transform.position;
                currentRotationAxis.z = 1f;
                currentRotation = Quaternion.AngleAxis(rotationAngle, currentRotationAxis);
            }

            return currentRotation;
        }

        private Vector3 GetNextPosition()
        {
            Vector3 nextPosition = GetRotation() * movementDirection;
            nextPosition *= sidewaysSpeed;
            nextPosition.z += forwardSpeed;
            nextPosition += transform.position;

            return nextPosition;
        }
        
        private Vector2 GetInitialSidewaysDirection()
        {
            var currentPosition = transform.position;
            float angleInRad = Mathf.Deg2Rad * StartingAngle;
            float x = Mathf.Cos(angleInRad);
            float y = Mathf.Sin(angleInRad);

            return new Vector2(x - currentPosition.x, y - currentPosition.y);
        }
        
        private void ResetFields()
        {
            movementDirection = GetInitialSidewaysDirection().normalized;
            currentAngle = 0f;
            canSwitch = switchValue != 0;
            stepCounter = 0;
            switchCounter = 0;
        }

#if UNITY_EDITOR
        
        public static Quaternion GetRotation(Vector3 currentPosition, float angle, bool globalAxisUsage, bool clockWiseRotation)
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

        public static Vector3 GetNextPosition(Vector3 currentPosition, Vector3 sidewaysDirection, Quaternion currentRotation, float sideSpeed, float forwardSpeed)
        {
            Vector3 nextPosition = currentRotation * sidewaysDirection;
            nextPosition *= sideSpeed * Time.fixedDeltaTime;
            nextPosition.z += forwardSpeed * Time.fixedDeltaTime;
            nextPosition += currentPosition;

            return nextPosition;
        }
        
        public static Vector3 GetInitialSidewaysDirection(Vector2 startingPosition, float startingAngle)
        {
            var currentPosition = startingPosition;
            float angleInRad = Mathf.Deg2Rad * startingAngle;
            float x = Mathf.Cos(angleInRad);
            float y = Mathf.Sin(angleInRad);

            return new Vector3(x - currentPosition.x, y - currentPosition.y);
        }
#endif
        
    }
}
