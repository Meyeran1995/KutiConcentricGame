using System.Collections;
using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player
{
    public class RadialPlayerMovement : MonoBehaviour
    {
        [Header("Values")] 
        [Range(0f, 360f)] public float StartingAngle;
        [SerializeField] private bool clockwise;

        [Header("Circle")]
        [SerializeField] private FloatValue angleGain;
        [SerializeField] private FloatValue radius;
        
        [Header("References")]
        [SerializeField] private Rigidbody2D playerRigidBody;
        
        private float currentAngle;
        private bool isMoving;

        public void Move(int givenDirection)
        {
            if (isMoving) return;
            
            if (givenDirection == 0)
            {
                currentAngle += clockwise ? -angleGain : angleGain;
            }
            else
            {
                float directedGain = angleGain * givenDirection;
                currentAngle += clockwise ? -directedGain : directedGain;
            }

            StartCoroutine(MoveRoutine());
        }

        private void OnEnable()
        {
            SnapToStartingAngle();
        }

        private void SnapToStartingAngle()
        {
            currentAngle = StartingAngle;
            var startingPosition = GetCirclePoint();
            playerRigidBody.position = startingPosition;
            playerRigidBody.rotation = StartingAngle;
        }
        
        private Vector2 GetCirclePoint()
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius.BaseValue * Mathf.Cos(angleInRad);
            float y = radius.BaseValue * Mathf.Sin(angleInRad);

            return new Vector2(x, y);
        }

        private IEnumerator MoveRoutine()
        {
            isMoving = true;
            var nextPosition = GetCirclePoint();
            playerRigidBody.MovePosition(nextPosition);
            playerRigidBody.MoveRotation(currentAngle);
            
            yield return new WaitForFixedUpdate();

            isMoving = false;
        }

#if UNITY_EDITOR

        [Header("Gizmos")]
        [SerializeField] private float gizmoRadius;

        public void EditorSnapToStartingPosition()
        {
            var playerTransform = transform;
            
            float z = playerTransform.position.z;
            Vector3 startingPos = GetCirclePoint(StartingAngle);
            startingPos.z = z;
            
            playerTransform.position = startingPos;
            playerTransform.rotation = Quaternion.Euler(0f, 0f, StartingAngle);
        }
        
        private void OnDrawGizmosSelected()
        {
            if(!angleGain) return;

            Gizmos.color = Color.grey;

            int numberOfPositions = Mathf.RoundToInt(360f /angleGain);

            for (int p = 0; p < numberOfPositions; p++)
            {
                Gizmos.DrawSphere(GetCirclePoint(StartingAngle + angleGain * p), gizmoRadius);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Vector3.zero, radius);
        }

        private Vector3 GetCirclePoint(float angle)
        {
            float angleInRad = Mathf.Deg2Rad * angle;
            float x = radius.BaseValue * Mathf.Cos(angleInRad);
            float y = radius.BaseValue * Mathf.Sin(angleInRad);

            return new Vector3(x, y);
        }

#endif
    }
}
