using System.Collections;
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
        
        [Header("Debug")]


        private float currentAngle;
        private bool isMoving;
        
        public void Move(int givenDirection)
        {
            if (isMoving) return;
            
            currentAngle += angleGain * givenDirection;

            StartCoroutine(MoveRoutine(givenDirection));
        }
        
        public void SnapToStartingAngle(float angle)
        {
            startingAngle = angle;
            currentAngle = startingAngle;
            playerRigidBody.position = GetCirclePoint();
            playerRigidBody.rotation = Quaternion.AngleAxis(startingAngle, Vector3.forward);
        }
        
        private Vector3 GetCirclePoint()
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius.BaseValue * Mathf.Cos(angleInRad);
            float y = radius.BaseValue * Mathf.Sin(angleInRad);

            return new Vector3(x, y, transform.position.z);
        }

        private IEnumerator MoveRoutine(int direction)
        {
            isMoving = true;
            var nextPosition = GetCirclePoint();
            
            yield return new WaitForFixedUpdate();

            playerRigidBody.MovePosition(nextPosition);
            playerRigidBody.MoveRotation(Quaternion.AngleAxis(currentAngle, Vector3.forward));
            
            isMoving = false;
            PlayerPositionTracker.MovePosition(this, direction);
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
