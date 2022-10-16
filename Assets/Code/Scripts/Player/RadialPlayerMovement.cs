using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.GameMode;
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
        [SerializeField] private Rigidbody playerRigidBody;
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
        
        [ReadOnly] public int movementDirection;

        public void StartMovement()
        {
            if(movementState is MovementStates.Moving) return;

            movementState = MovementStates.Moving;
            velocityCalculator.StartMovement();
        }

        public void Brake()
        {
            movementState = MovementStates.Brake;
            velocityCalculator.StartBrake();
        }

        public void SnapToStartingAngle(float angle)
        {
            startingAngle = angle;
            currentAngle = startingAngle;
            playerRigidBody.position = GetCirclePoint();
            playerRigidBody.rotation = Quaternion.AngleAxis(startingAngle, Vector3.forward);
            collisionHelper.FaceSpawn();
        }

        private void FixedUpdate()
        {
            if(movementState is MovementStates.None) return;
            Move();
        }

        private void Move()
        {
            float currentVelocity = velocityCalculator.GetVelocity();

            if (movementState is MovementStates.Brake && currentVelocity <= 0f)
            {
                movementState = MovementStates.None;
                return;
            }
            
            currentAngle += currentVelocity * angleGain * movementDirection;

            var nextPosition = GetCirclePoint();
            
            playerRigidBody.MovePosition(nextPosition);
            playerRigidBody.MoveRotation(Quaternion.AngleAxis(currentAngle, Vector3.forward));
            
            PlayerPositionTracker.MovePosition(this, movementDirection);
        }
        
        private Vector3 GetCirclePoint()
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius.BaseValue * Mathf.Cos(angleInRad);
            float y = radius.BaseValue * Mathf.Sin(angleInRad);

            return new Vector3(x, y, transform.position.z);
        }

#if UNITY_EDITOR

        [Header("Gizmos")]
        [SerializeField] private float gizmoRadius;
        [SerializeField] private Transform centerTransform;
        
        
        public float Radius => radius.BaseValue;

        public void EditorSnapToStartingPosition()
        {
            var playerTransform = transform;
            Vector3 startingPos = GetCirclePoint(startingAngle);
            
            playerTransform.position = startingPos + centerTransform.position;
            playerTransform.rotation = Quaternion.Euler(0f, 0f, startingAngle);
        }
        
        private void OnDrawGizmosSelected()
        {
            if(!angleGain) return;

            Gizmos.color = Color.grey;

            int numberOfPositions = Mathf.RoundToInt(360f /angleGain);

            for (int p = 0; p < numberOfPositions; p++)
            {
                Gizmos.DrawSphere(GetCirclePoint(startingAngle + angleGain * p), gizmoRadius);
            }
        }

        private Vector3 GetCirclePoint(float angle)
        {
            float angleInRad = Mathf.Deg2Rad * angle;
            float x = radius.BaseValue * Mathf.Cos(angleInRad);
            float y = radius.BaseValue * Mathf.Sin(angleInRad);

            return new Vector3(x, y, transform.position.z);
        }

#endif
    }
}
