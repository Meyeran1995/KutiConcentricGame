using System;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Splines
{
    public class SplineFollower : MonoBehaviour
    {
        [field: Header("Debug"), ReadOnly, SerializeField] public bool IsPlaying { get; private set; }

        [SerializeField, ReadOnly] private SplineContainer splineContainer;
        [SerializeField, ReadOnly] private CurveParameter speedCurve;
        [SerializeField, ReadOnly] private float progress;
        [SerializeField, ReadOnly] private float currentSpeed;
        
        private float baseSpeed;

        public event Action EndOfSplineReached;

        public void SetUpSpline(SplineContainer spline, CurveParameter speedChange)
        {
            splineContainer = spline;
            speedCurve = speedChange;
        }

        public SplineContainer GetTargetSpline()
        {
            return splineContainer;
        }
        
        public void SetBaseSpeed(float speed)
        {
            baseSpeed = speed;
        }
        
        public void Restart()
        {
            currentSpeed = baseSpeed;
            progress = 0f;
            UpdatePosition();

            IsPlaying = true;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        private void OnDisable()
        {
            EndOfSplineReached = null;
        }

        private void Update() 
        {
            if(!IsPlaying) return;

            currentSpeed = speedCurve.Evaluate(progress) * baseSpeed;
            progress += currentSpeed * Time.deltaTime;
            
            if (progress >= 1f)
            {
                IsPlaying = false;
                EndOfSplineReached?.Invoke();
                return;
            }
            
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.localPosition = splineContainer.EvaluatePosition(progress);
        }
    }
}