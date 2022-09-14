using Meyham.DataObjects;
using Meyham.Items;
using UnityEngine;

namespace Meyham.EditorHelpers
{
    public class ItemMovementPrediction : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] private ItemMovementStatsSO debugStats;
        [SerializeField] private float zMovement;
        [SerializeField, Range(0f, 360f)] private float startingAngle;
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
        private float currentAngle, currentRotationGain, currentSidewaysSpeed;
        private int timesSwitched;

        private void Start()
        {
            startPosition = transform.position;
            gameIsRunning = true;
            movementStats = movement == null ? debugStats : movement.Stats;
        }

        private void OnEnable()
        {
            if(movement == null) return;

            startingAngle = movement.StartingAngle;
        }

        private void ApplySwitchModifiers()
        {
            currentRotationGain += movementStats.RotationGainModifier;
            currentSidewaysSpeed += movementStats.SidewaysSpeedModifier;

            if (movementStats.AxisSwitch)
                useGlobal = !useGlobal;

            if (movementStats.RotationDirectionSwitch)
                clockwiseRotation = !clockwiseRotation;

            timesSwitched++;
            canSwitch = timesSwitched != movementStats.TimesToSwitch;
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos) return;
            if (predictionSteps == 0 || resolution == 0 || gizmoSphereRadius == 0f) return;
            if (movementStats == null) return;

            InitializeFields();
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(currentPosition, gizmoSphereRadius);
            DrawPositions();
        }
        
        private void InitializeFields()
        {
            currentPosition = gameIsRunning ? startPosition : transform.position;
            direction = ItemMovement.GetInitialSidewaysDirection(currentPosition, startingAngle).normalized;

            currentAngle = 0f;
            timesSwitched = 0;
            canSwitch = movementStats.Switch != 0;

            currentRotationGain = movementStats.RotationGain;
            currentSidewaysSpeed = movementStats.SidewaysSpeed;
            zMovement = movementStats.ForwardSpeed;
            
            useGlobal = movementStats.UseGlobalAxis;
            clockwiseRotation = movementStats.ClockwiseRotation;
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
                
                currentPosition = ItemMovement.GetNextPosition(currentPosition, direction,
                    ItemMovement.GetRotation(currentPosition, currentAngle, useGlobal,
                        clockwiseRotation), currentSidewaysSpeed, zMovement);

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
        
        private void OnValidate()
        {
            if (TryGetComponent(out movement))
            {
                movementStats = movement.Stats;
                startingAngle = movement.StartingAngle;
            }
            else
            {
                movementStats = debugStats;
            }
        }

#endif
    }
}
