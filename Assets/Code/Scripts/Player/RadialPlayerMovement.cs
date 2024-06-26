using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class RadialPlayerMovement : MonoBehaviour
    {
        [Header("Circle")]
        [SerializeField] private FloatParameter angleGain;
        [SerializeField] private FloatParameter radius;
        
        [Header("References")]
        [SerializeField] private PlayerVelocityCalculator velocityCalculator;
    
        [Header("Debug")]
        [SerializeField, ReadOnly] private float startingAngle;
        [Space]
        [SerializeField, ReadOnly] private int movementDirection;
        [SerializeField, ReadOnly] private int brakeDirection;
        [Space]
        [SerializeField, ReadOnly] private float currentAngle;
        [SerializeField, ReadOnly] private MovementStates movementState;

        public float CirclePosition => currentAngle;
        
        public int MovementDirection => movementDirection;
        
        private const int BaseMovementDirection = -1;

        private enum MovementStates
        {
            None,
            Moving,
            Brake
        }

        public void FlipMovementDirection()
        {
            movementDirection = -movementDirection;
        }

        public void ResetMovementDirection()
        {
            movementDirection = BaseMovementDirection;
        }
        
        public void StartMovement()
        {
            if(movementState is MovementStates.Moving) return;

            movementState = MovementStates.Moving;
            velocityCalculator.StartMovement();
        }

        public void Brake()
        {
            brakeDirection = movementDirection;
            movementState = MovementStates.Brake;
            velocityCalculator.StartBrake();
        }

        public void SnapToStartingAngle(float angle)
        {
            startingAngle = angle;
            currentAngle = startingAngle;
            transform.SetPositionAndRotation(GetCirclePoint(), Quaternion.AngleAxis(startingAngle, Vector3.forward));
        }

        private void Update()
        {
            if(movementState is MovementStates.None) return;
            Move();
        }

        private void OnEnable()
        {
            movementDirection = BaseMovementDirection;
            movementState = MovementStates.None;
            velocityCalculator.enabled = true;
        }

        private void OnDisable()
        {
            velocityCalculator.enabled = false;
        }

        private void Move()
        {
            var currentVelocity = velocityCalculator.GetVelocity();

            if (movementState is MovementStates.Brake)
            {
                if (currentVelocity <= 0f)
                {
                    movementState = MovementStates.None;
                    return;
                }
                
                currentAngle += currentVelocity * angleGain * brakeDirection;
            }
            else
            {
                currentAngle += currentVelocity * angleGain * movementDirection;
            }

            ClampAngle();
            
            transform.SetPositionAndRotation(GetCirclePoint(), Quaternion.AngleAxis(currentAngle, Vector3.forward));
        }

        private Vector3 GetCirclePoint()
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);

            return new Vector3(x, y, transform.position.z);
        }

        private void ClampAngle()
        {
            switch (currentAngle)
            {
                case < 0f:
                {
                    currentAngle += 360f;
                    return;
                }
                case > 360f:
                {
                    currentAngle -= 360f;
                    return;
                }
                default:
                    return;
            }
        }
    }
}
