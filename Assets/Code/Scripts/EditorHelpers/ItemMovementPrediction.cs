using Meyham.DataObjects;
using Meyham.Items;
using UnityEngine;

namespace Meyham.EditorHelpers
{
    public class ItemMovementPrediction : MonoBehaviour
    {
#if UNITY_EDITOR
        
        [Header("Gizmos")] 
        [SerializeField] private bool showGizmos = true;
        [SerializeField, Min(0)] private int predictionSteps;
        [SerializeField, Min(0)] private int resolution;
        [SerializeField, Min(0f)] private float gizmoSphereRadius;

        private const int stepsToSecond = 50;
        
        // prediction references
        private ItemMovementStatsSO movementStats;
        private ItemMovement movement;

        // playmode fields
        private bool gameIsRunning;
        private Vector3 startPosition;

        // gizmo fields
        private bool useGlobal, clockwiseRotation, canSwitch;
        private Vector3 currentPosition, direction;
        private float currentAngle, currentRotationGain, currentSpeed;
        private int timesSwitched;

        private void Start()
        {
            gameIsRunning = true;
            useGlobal = movementStats.UseGlobalAxis;
            clockwiseRotation = movementStats.ClockwiseRotation;
            
            startPosition = transform.position;
            
            currentAngle = currentRotationGain * Time.fixedDeltaTime;
            direction = movement.GetStartDirection(currentPosition).normalized;
        }
        
        private void ApplySwitchModifiers()
        {
            currentRotationGain += movementStats.RotationGainModifier;
            currentSpeed += movementStats.SpeedModifier;

            if (movementStats.AxisSwitch)
                useGlobal = !useGlobal;

            if (movementStats.RotationDirectionSwitch)
                clockwiseRotation = !clockwiseRotation;

            timesSwitched++;
            canSwitch = timesSwitched != movementStats.TimesToSwitch;
        }

        private void OnDrawGizmos()
        {
            if(!showGizmos) return;
            if(predictionSteps == 0 || resolution == 0 || gizmoSphereRadius == 0f) return;

            if (movementStats == null)
                movementStats = movement.Stats;
            
            InitializeFields();

            DrawPositions();
        }
        
        private void InitializeFields()
        {
            if (gameIsRunning)
            {
                currentPosition = startPosition;
            }
            else
            {
                currentPosition = transform.position;
                direction = movement.GetStartDirection(currentPosition).normalized;
            }
            
            currentRotationGain = movementStats.RotationGain;
            currentSpeed = movementStats.Speed;
            currentAngle = 0f;

            useGlobal = movementStats.UseGlobalAxis;
            clockwiseRotation = movementStats.ClockwiseRotation;
            timesSwitched = 0;
            canSwitch = movementStats.Switch != 0;
        }

        private void DrawPositions()
        {
            for (int i = 1; i <= predictionSteps; i++) // start at 1 to avoid 0 % movement.Switch == 0 mistake
            {
                currentAngle += currentRotationGain * Time.fixedDeltaTime;

                if ((int)currentAngle > movementStats.MaxAngle)
                {
                    currentAngle = movementStats.MaxAngle;
                }
                
                currentPosition = movement.GetNextPosition(currentPosition, direction,
                    movement.GetRotation(currentPosition, currentAngle, useGlobal, clockwiseRotation), currentSpeed);

                if ((int)currentAngle == movementStats.MaxAngle)
                {
                    currentAngle = 0f;
                }
                
                if (canSwitch && i % movementStats.Switch == 0)
                    ApplySwitchModifiers();

                DrawPosition(i);
            }
        }
        
        private void DrawPosition(int index)
        {
            if (index != 0 && index % stepsToSecond == 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(currentPosition, gizmoSphereRadius);
                return;
            }
                
            if(index % resolution != 0) return;
                
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(currentPosition, gizmoSphereRadius);
        }

        private void OnApplicationQuit() => gameIsRunning = false;
        private void OnValidate() => movement = GetComponent<ItemMovement>();

        public void SetUpPrediction(ItemMovement movement)
        {
            this.movement = movement;
            movementStats = movement.Stats;
        }

#endif
    }
}
