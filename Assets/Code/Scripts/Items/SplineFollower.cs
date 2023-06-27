using System;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class SplineFollower : MonoBehaviour
    {
        [field: Header("Debug"), ReadOnly, SerializeField] public bool IsPlaying { get; private set; }
        public event Action EndOfSplineReached;

        [SerializeField, ReadOnly] private SplineContainer splineContainer;
        [SerializeField, ReadOnly] private SpeedPointContainer speedContainer;
        [SerializeField, ReadOnly] private float progress;
        [SerializeField, ReadOnly] private float currentSpeed;
        [SerializeField, ReadOnly] private bool usesSpeedPoints;

        private float baseSpeed;

        public void SetSpline(SplineContainer spline)
        {
            splineContainer = spline;
            usesSpeedPoints = splineContainer.TryGetComponent(out speedContainer);
        }

        public SplineContainer GetTargetSpline()
        {
            return splineContainer;
        }
        
        public void SetBaseSpeed(float speed)
        {
            baseSpeed = speed;
            currentSpeed = speed;
        }
        
        public void Restart(bool autoPlay)
        {
            progress = 0f;
            SetPosition(0f);
            
            if(!autoPlay) return;
            
            Play();
        }

        public void Play()
        {
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
            
            progress += Time.deltaTime / currentSpeed;
            
            if (progress >= 1f)
            {
                IsPlaying = false;
                EndOfSplineReached?.Invoke();
                return;
            }
            UpdatePosition();
            
            if(!usesSpeedPoints) return;

            if(!speedContainer.WasNewSpeedPointReached(progress, out var speedModifier)) return;

            currentSpeed = baseSpeed * speedModifier;
        }

        private void UpdatePosition()
        {
            transform.localPosition = splineContainer.EvaluatePosition(progress);
        }
        
        private void SetPosition(float splineProgress)
        {
            transform.localPosition = splineContainer.EvaluatePosition(splineProgress);
        }
    }
}