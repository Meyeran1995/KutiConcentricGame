using System;
using DG.Tweening;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Splines
{
    public class SplineFollower : MonoBehaviour
    {
        [field: Header("Debug"), ReadOnly, SerializeField] public bool IsPlaying { get; private set; }

        [SerializeField, ReadOnly] private SplineContainer splineContainer;
        [SerializeField, ReadOnly] private SpeedPointContainer speedContainer;
        [SerializeField, ReadOnly] private float progress;
        [SerializeField, ReadOnly] private float currentSpeed;
        
        private float baseSpeed;

        private Tweener activeTween;
        
        public event Action EndOfSplineReached;

        public void SetUpSpline(SplineContainer spline, SpeedPoint[] speedPoints)
        {
            splineContainer = spline;
            speedContainer = splineContainer.GetComponent<SpeedPointContainer>();
            speedContainer.SetSpeedPoints(speedPoints);
        }

        public SplineContainer GetTargetSpline()
        {
            return splineContainer;
        }
        
        public void SetBaseSpeed(float speed)
        {
            baseSpeed = speed;
        }
        
        public void Restart(bool autoPlay)
        {
            currentSpeed = baseSpeed;
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
            
            if (!activeTween.IsActive()) return;
            
            activeTween.Kill(true);
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
            
            if(!speedContainer.WasNewSpeedPointReached(progress, out var speedModifier)) return;

            if (activeTween.IsActive())
            {
                activeTween.Kill(true);
            }
            
            activeTween = DOTween.To(() => currentSpeed, speed => currentSpeed = speed, baseSpeed * speedModifier, 2f);
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