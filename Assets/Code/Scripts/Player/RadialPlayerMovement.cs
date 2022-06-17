using System.Collections;
using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player
{
    public class RadialPlayerMovement : MonoBehaviour
    {
        [Header("Values")] 
        [SerializeField, Range(0f, 360f)] private float startingAngle;
        [SerializeField] private bool snapToStartOnAwake, clockwise;

        [Header("Circle")]
        [SerializeField] private FloatValue angleGain;
        [SerializeField] private FloatValue radius;
        [SerializeField] private Transform center;
        
        [Header("References")]
        [SerializeField] private Rigidbody2D playerRigidBody;
        
        private float currentAngle;
        private bool isMoving;

        private void Awake()
        {
            currentAngle = startingAngle;
            
            if(!snapToStartOnAwake) return;
            
            var startingPosition = GetCirclePoint();
            playerRigidBody.position = startingPosition;
            playerRigidBody.rotation = startingAngle;
        }

        public void Move(int givenDirection)
        {
            if (isMoving) return;
            
            if (givenDirection == 0)
            {
                currentAngle += clockwise ? angleGain : -angleGain;
            }
            else
            {
                float directedGain = angleGain * givenDirection;
                currentAngle += clockwise ? directedGain : -directedGain;
            }

            StartCoroutine(MoveRoutine());
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
        
        private void OnDrawGizmosSelected()
        {
            if(!angleGain) return;

            Gizmos.color = Color.grey;

            int numberOfPositions = Mathf.RoundToInt(360f /angleGain);

            for (int p = 0; p < numberOfPositions; p++)
            {
                Gizmos.DrawSphere(GetCirclePoint(angleGain.BaseValue * p), gizmoRadius);
            }
        }
        
        private void OnDrawGizmos()
        {
            if(!center) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center.position, radius);
        }

        
        public void EditorSnapToStartingPosition()
        {
            var playerTransform = transform;
            
            float z = playerTransform.position.z;
            Vector3 startingPos = GetCirclePoint(startingAngle);
            startingPos.z = z;
            
            playerTransform.position = startingPos;
            playerTransform.rotation = Quaternion.Euler(0f, 0f, startingAngle);
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
