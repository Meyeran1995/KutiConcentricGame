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
        [SerializeField] private PlayerController controller;
    
        private float currentAngle;
        private MovementStates movementState;

        private enum MovementStates
        {
            None,
            Moving,
            Brake
        }
        
        [ReadOnly] public int movementDirection, brakeDirection;

        public Vector3 LastPosition { get; private set; }
        
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

            LastPosition = playerRigidBody.position;
            // ClampAngle();
            var nextPosition = GetCirclePoint();
            
            playerRigidBody.MovePosition(nextPosition);
            playerRigidBody.MoveRotation(Quaternion.AngleAxis(currentAngle, Vector3.forward));
            
            PlayerPositionTracker.MovePosition(controller);
        }

        // private void ClampAngle()
        // {
        //     if (currentAngle < 0f)
        //     {
        //         currentAngle += 360f;
        //         return;
        //     }
        //     
        //     if(currentAngle <= 360f) return;
        //
        //     currentAngle -= 360f;
        // }
        
        private Vector3 GetCirclePoint()
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius.BaseValue * Mathf.Cos(angleInRad);
            float y = radius.BaseValue * Mathf.Sin(angleInRad);

            return new Vector3(x, y, transform.position.z);
        }
    }
}
