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
        private bool clockWise;

        private void Awake()
        {
            currentAngle = startingAngle;
            playerRigidBody.position = GetCirclePoint(startingAngle);
        }

        public void Move(int givenDirection)
        {
            if (givenDirection == 0)
            {
                currentAngle += clockWise ? angleGain : -angleGain;
            }
            else
            {
                currentAngle += givenDirection * angleGain;
            }
            
            Move();
        }

        private void Move()
        {
            playerRigidBody.MovePosition(GetCirclePoint());
        }
        
        private Vector2 GetCirclePoint()
        {
            float angleInRad = Mathf.Deg2Rad * currentAngle;
            float x = radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);

            return new Vector2(x, y);
        }
        
        private Vector2 GetCirclePoint(float angle)
        {
            float angleInRad = Mathf.Deg2Rad * angle;
            float x = radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);

            return new Vector2(x, y);
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

#endif
    }
}
