using UnityEngine;

namespace Meyham.Items
{
    public class ItemMovement : MonoBehaviour
    {
        // Speed
        [field: Header("Speed"), SerializeField] public float Speed { get; private set; }
        
        // Size
        [field: Header("Size"), SerializeField, Min(0f)] public float SizeGain { get; private set; }
        [field: SerializeField] public float MaxSize { get; private set; }
        
        // Rotation
        [field: Header("Rotation"), SerializeField] public bool UseGlobalAxis { get; private set; }
        [field: SerializeField] public bool ClockwiseRotation { get; private set; }
        [field: SerializeField, Range(0f, 360f)] public float RotationGain { get; private set; }
        [field: SerializeField, Range(0f, 360f)] public float StartingAngle { get; private set; }
        [field: SerializeField, Range(0, 360)] public int MaxAngle { get; private set; }
        
        // Switch
        [field: Header("Switch Behaviour"), SerializeField, Min(0)] public int Switch { get; private set; }
        [field: SerializeField, Min(1)] public int TimesToSwitch { get; private set; }
        [field: SerializeField, Range(-360f, 360f)] public float RotationGainModifier { get; private set; }
        [field: SerializeField] public float SpeedModifier { get; private set; }
        [field: SerializeField] public bool AxisSwitch { get; private set; }
        [field: SerializeField] public bool RotationDirectionSwitch { get; private set; }

        private Vector3 movementDirection, sizeIncrease;
        private float currentAngle;
        private int stepCounter, switchCounter;
        private bool maxSizeReached, canSwitch;

        private void Awake()
        {
            movementDirection = GetStartDirection(transform.position).normalized;
            sizeIncrease = new Vector3(SizeGain, SizeGain);
            currentAngle = RotationGain * Time.deltaTime;
            canSwitch = Switch != 0;
        }

        private void FixedUpdate()
        {
            currentAngle += RotationGain * Time.fixedDeltaTime;

            if (!maxSizeReached)
            {
                transform.localScale += sizeIncrease * Time.fixedDeltaTime;
                maxSizeReached = transform.localScale.x >= MaxSize;

                if (maxSizeReached)
                    transform.localScale = new Vector3(MaxSize, MaxSize, MaxSize);
            }
            
            Move();
        }

        private void Move()
        {
            Vector3 currentPosition = transform.position;
            var currentRotation = GetRotation(currentPosition, currentAngle, UseGlobalAxis, ClockwiseRotation);
            transform.position = GetNextPosition(currentPosition, movementDirection, currentRotation, Speed);

            if ((int)currentAngle == MaxAngle)
            {
                currentAngle = RotationGain * Time.deltaTime;
            }

            if(!canSwitch) return;
            
            stepCounter++;
            
            if(stepCounter % Switch != 0) return;

            switchCounter++;
            
            ApplySwitchModifiers();
            if (switchCounter == TimesToSwitch)
                canSwitch = false;
            
            stepCounter = 0;
        }

        private void ApplySwitchModifiers()
        {
            RotationGain += RotationGainModifier;
            Speed += SpeedModifier;

            if (AxisSwitch)
                UseGlobalAxis = !UseGlobalAxis;

            if (RotationDirectionSwitch)
                ClockwiseRotation = !ClockwiseRotation;
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
    }
}
