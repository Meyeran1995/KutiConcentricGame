using System.Collections;
using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player
{
    public class RadialPlayerMovement : MonoBehaviour
    {
        [Header("Values")] 
        [SerializeField] private float startingAngle;


        [Header("Circle")]
        [SerializeField] private FloatValue angleGain;
        [SerializeField] private FloatValue radius;
        [SerializeField] private Transform center;
        
        [Header("References")]
        [SerializeField] private Rigidbody2D playerRigidBody;
        

        private float currentAngle;
        private bool clockWise, isMoving;

        private void Awake()
        {
            currentAngle = startingAngle;
            playerRigidBody.position = GetCirclePoint();
            LookAtCenter();
        }

        public void Move(int givenDirection)
        {
            if (isMoving) return;
            
            if (givenDirection == 0)
            {
                currentAngle += clockWise ? -angleGain : angleGain;
            }
            else
            {
                currentAngle += givenDirection * angleGain;
            }

            StartCoroutine(MoveRoutine());
        }

        private Vector2 GetCirclePoint()
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);

            return new Vector2(x, y);
        }

        private void LookAtCenter()
        {
            var playerTransform = transform;
            Vector2 directionToCenter = center.position - playerTransform.position;
            var lookAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
            
            playerRigidBody.MoveRotation(lookAngle);
        }

        private IEnumerator MoveRoutine()
        {
            isMoving = true;
            playerRigidBody.MovePosition(GetCirclePoint());

            yield return new WaitForFixedUpdate();

            LookAtCenter();
            isMoving = false;
        }

#if UNITY_EDITOR

        [Header("Gizmos")]
        [SerializeField] private float gizmoRadius;
        
        private void OnDrawGizmosSelected()
        {
            if(!center || !angleGain) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center.position, radius);
            
            Gizmos.color = Color.grey;
            float nextAngle = angleGain;

            int numberOfPositions = Mathf.RoundToInt(360f /angleGain);

            for (int p = 0; p < numberOfPositions; p++)
            {
                Gizmos.DrawSphere(GetCirclePoint(angleGain * p), gizmoRadius);
            }
        }

        
        public void EditorSnapToStartingPosition()
        {
            float z = transform.position.z;
            Vector2 startingPos = GetCirclePoint(startingAngle);
            transform.position = new Vector3(startingPos.x, startingPos.y, z);
        }
        
        private Vector2 GetCirclePoint(float angle)
        {
            float angleInRad = Mathf.Deg2Rad * angle;
            float x = radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);

            return new Vector2(x, y);
        }

#endif
    }
}
