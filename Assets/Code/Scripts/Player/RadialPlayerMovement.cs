using Meyham.Collision;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class RadialPlayerMovement : MonoBehaviour
    {
        [Header("Values")] 
        [SerializeField, Range(0f, 360f)] private float startingAngle;
        
        [Header("Circle")]
        [SerializeField] private FloatValue angleGain;
        [SerializeField] private FloatValue radius;
        
        [Header("References")]
        [SerializeField] private PlayerCollisionHelper collisionHelper;
        [SerializeField] private PlayerVelocityCalculator velocityCalculator;
    
        private float currentAngle;
        private MovementStates movementState;

        private enum MovementStates
        {
            None,
            Moving,
            Brake
        }
        
        [ReadOnly] public int movementDirection, brakeDirection;

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
            collisionHelper.FaceSpawn();
        }

        private void Update()
        {
            if(movementState is MovementStates.None) return;
            Move();
        }

        private void OnEnable()
        {
            velocityCalculator.enabled = true;
        }

        private void OnDisable()
        {
            velocityCalculator.enabled = false;
        }

        private void Move()
        {
            float currentVelocity = velocityCalculator.GetVelocity();

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

            var nextPosition = GetCirclePoint();
            
            transform.SetPositionAndRotation(nextPosition, Quaternion.AngleAxis(currentAngle, Vector3.forward));
        }

        private Vector3 GetCirclePoint()
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius.BaseValue * Mathf.Cos(angleInRad);
            float y = radius.BaseValue * Mathf.Sin(angleInRad);

            return new Vector3(x, y, transform.position.z);
        }
    }
}
