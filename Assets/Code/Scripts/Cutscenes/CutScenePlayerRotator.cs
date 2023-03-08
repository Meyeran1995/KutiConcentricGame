using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Unity.Mathematics;
using UnityEngine;

namespace Meyham.Cutscenes
{
    public class CutScenePlayerRotator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform outerPivot;
        
        [Header("Values")]
        [SerializeField] private FloatValue rotationSpeed;
        [SerializeField] private FloatValue outerStartingRotation;
        
        [Header("Debug")]
        [SerializeField, ReadOnly] private float lerpOuter;
        [SerializeField, ReadOnly] private bool shouldRotateOuter;
        [Space]
        [SerializeField, ReadOnly] private float lerpInner;
        [SerializeField, ReadOnly] private bool shouldRotateInner;
        
        private Quaternion desiredRotationOuter, desiredRotationInner;
        
        private Quaternion currentRotationOuter, currentRotationInner;

        public void RotateIntoCircle()
        {
            desiredRotationOuter = Quaternion.identity;

            shouldRotateOuter = true;
            lerpOuter = 0f;
            enabled = true;
        }

        public void RotateOutOfCircle()
        {
            desiredRotationOuter = Quaternion.AngleAxis(outerStartingRotation, Vector3.forward);

            shouldRotateOuter = true;
            lerpOuter = 0f;
            enabled = true;
        }

        public void RotateTowardsCircleAngle(float angle)
        {
            desiredRotationInner = Quaternion.AngleAxis(angle, Vector3.forward);

            if (desiredRotationInner == currentRotationInner)
            {
                return;
            }
            
            shouldRotateInner = true;
            lerpInner = 0f;
            enabled = true;
        }
        
        private void Awake()
        {
            currentRotationInner = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            currentRotationOuter = Quaternion.AngleAxis(outerStartingRotation, Vector3.forward);
            outerPivot.localRotation = currentRotationOuter;
        }

        private void Update()
        {
            if (!shouldRotateOuter && !shouldRotateInner)
            {
                enabled = false;
                return;
            }
            
            if (shouldRotateInner)
            {
                lerpInner += Time.deltaTime * rotationSpeed.RuntimeValue;

                if (lerpInner > 1f)
                {
                    lerpInner = 1f;
                    shouldRotateInner = false;
                }
                
                currentRotationInner = Quaternion.Lerp(currentRotationInner, desiredRotationInner, lerpInner);

                transform.localRotation = currentRotationInner;
            }
            
            if(!shouldRotateOuter) return;
            
            lerpOuter += Time.deltaTime * rotationSpeed.RuntimeValue;

            if (lerpOuter > 1f)
            {
                lerpOuter = 1f;
                shouldRotateOuter = false;
            }
                
            currentRotationOuter = Quaternion.Lerp(currentRotationOuter, desiredRotationOuter, lerpOuter);

            outerPivot.localRotation = currentRotationOuter;
        }
    }
}
