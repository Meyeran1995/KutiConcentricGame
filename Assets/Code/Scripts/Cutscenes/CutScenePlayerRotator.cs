using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Cutscenes
{
    public class CutScenePlayerRotator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform outerPivot;
        
        [Header("Values")]
        [SerializeField] private FloatParameter rotationSpeed;
        [SerializeField] private FloatParameter outerStartingRotation;
        
        [Header("Debug")]
        [SerializeField, ReadOnly] private float lerpOuter;
        [SerializeField, ReadOnly] private bool shouldRotateOuter;
        [Space]
        [SerializeField, ReadOnly] private float lerpInner;
        [SerializeField, ReadOnly] private bool shouldRotateInner;
        
        private Quaternion desiredRotationOuter, desiredRotationInner;
        
        private Quaternion currentRotationOuter, currentRotationInner;

        public bool IsRotating()
        {
            return shouldRotateInner || shouldRotateOuter;
        }

        public void StartRotationIntoCircle()
        {
            desiredRotationOuter = Quaternion.identity;

            shouldRotateOuter = true;
            lerpOuter = 0f;
            enabled = true;
        }

        public void StartRotationOutOfCircle()
        {
            desiredRotationOuter = Quaternion.AngleAxis(outerStartingRotation, Vector3.forward);

            shouldRotateOuter = true;
            lerpOuter = 0f;
            enabled = true;
        }

        public void StartRotationTowardsCircleAngle(float angle)
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
        
        public void SetInnerRotationInstant(float angle)
        {
            desiredRotationInner = Quaternion.AngleAxis(angle, Vector3.forward);

            lerpInner = 0f;
            currentRotationInner = desiredRotationInner;
            transform.localRotation = currentRotationInner;
        }
        
        public void RotateOutOfCircleInstant()
        {
            desiredRotationOuter = Quaternion.AngleAxis(outerStartingRotation, Vector3.forward);

            lerpOuter = 0f;
            currentRotationOuter = desiredRotationOuter;
            outerPivot.localRotation = desiredRotationOuter;
        }
        
        private void Start()
        {
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
