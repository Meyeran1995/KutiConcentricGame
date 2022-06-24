using Meyham.Items;
using UnityEngine;

namespace Meyham.EditorHelpers
{
    public class ItemMovementPrediction : MonoBehaviour
    {
#if UNITY_EDITOR
        
        [Header("Gizmos")]
        [SerializeField, Min(0)] private int predictionSteps;
        [SerializeField, Min(0)] private int resolution;
        [SerializeField, Min(0f)] private float gizmoSphereRadius;

        private const int stepsToSecond = 50;

      
        private ItemMovement movement;

        // playmode fields
        private bool gameIsRunning, startUseGlobal, startClockwiseRotation;
        private Vector3 startPosition;
        private float startSize, startRotationGain, startSpeed;

        // gizmo fields
        private bool useGlobal, clockwiseRotation, canSwitch;
        private Vector3 currentPosition, direction;
        private float currentSize, currentAngle, currentRotationGain, currentSpeed;
        private int timesSwitched;
        
        private void Start()
        {
            gameIsRunning = true;
            useGlobal = movement.UseGlobalAxis;
            clockwiseRotation = movement.ClockwiseRotation;
            
            startPosition = transform.position;
            startSize = transform.localScale.x;
            startRotationGain = movement.RotationGain;
            startSpeed = movement.Speed;
            startClockwiseRotation = movement.ClockwiseRotation;
            startUseGlobal = movement.UseGlobalAxis;
            
            currentAngle = currentRotationGain * Time.fixedDeltaTime;
            direction = movement.GetStartDirection(currentPosition).normalized;
        }
        
        private void ApplySwitchModifiers()
        {
            currentRotationGain += movement.RotationGainModifier;
            currentSpeed += movement.SpeedModifier;

            if (movement.AxisSwitch)
                useGlobal = !useGlobal;

            if (movement.RotationDirectionSwitch)
                clockwiseRotation = !clockwiseRotation;

            timesSwitched++;
            canSwitch = timesSwitched != movement.TimesToSwitch;
        }

        private void InitializeFields()
        {
            if (gameIsRunning)
            {
                currentPosition = startPosition;
                currentSize = startSize;
                currentRotationGain = startRotationGain;
                currentSpeed = startSpeed;
                
                useGlobal = startUseGlobal;
                clockwiseRotation = startClockwiseRotation;
            }
            else
            {
                currentPosition = transform.position;
                currentSize = transform.localScale.x;
                currentRotationGain = movement.RotationGain;
                currentSpeed = movement.Speed;
                
                useGlobal = movement.UseGlobalAxis;
                clockwiseRotation = movement.ClockwiseRotation;
                
                direction = movement.GetStartDirection(currentPosition).normalized;
            }

            currentAngle = currentRotationGain * Time.fixedDeltaTime;
            timesSwitched = 0;
            canSwitch = movement.Switch != 0;
        }
        
        private void OnDrawGizmosSelected()
        {
            if(predictionSteps == 0 || resolution == 0 || gizmoSphereRadius == 0f) return;
            
            InitializeFields();

            DrawPositions();
        }

        private void DrawPositions()
        {
            for (int i = 1; i <= predictionSteps; i++) // start at 1 to avoid 0 % movement.Switch == 0 mistake
            {
                currentAngle += currentRotationGain * Time.fixedDeltaTime;
                
                if(currentSize < movement.MaxSize)
                    currentSize += movement.SizeGain * Time.fixedDeltaTime;
                
                currentPosition = movement.GetNextPosition(currentPosition, direction,
                    movement.GetRotation(currentPosition, currentAngle, useGlobal, clockwiseRotation), currentSpeed);

                if ((int)currentAngle == movement.MaxAngle)
                {
                    currentAngle = currentRotationGain * Time.fixedDeltaTime;
                }
                
                if (canSwitch && i % movement.Switch == 0)
                    ApplySwitchModifiers();

                DrawPosition(i);
            }
        }
        
        private void DrawPosition(int index)
        {
            if (index != 0 && index % stepsToSecond == 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(currentPosition, currentSize * gizmoSphereRadius);
                return;
            }
                
            if(index % resolution != 0) return;
                
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(currentPosition, currentSize * gizmoSphereRadius);
        }

        private void OnApplicationQuit()
        {
            gameIsRunning = false;
        }

        private void OnValidate()
        {
            movement = GetComponent<ItemMovement>();
        }
#endif
    }
}
