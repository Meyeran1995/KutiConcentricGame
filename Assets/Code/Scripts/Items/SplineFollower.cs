using System;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class SplineFollower : MonoBehaviour
    {
        [SerializeField] private FloatValue speed;
        [field: Header("Debug"), ReadOnly, SerializeField] public bool IsPlaying { get; private set; }
        [ReadOnly] public SplineContainer SplineContainer;

        public event Action EndOfSplineReached;

        [SerializeField, ReadOnly] private float progress;
        private float progressIncrement;
        
        public void Restart(bool autoPlay)
        {
            progress = 0f;
            SetPosition(0f);
            
            if(!autoPlay) return;
            
            Play();
        }

        public void Play()
        {
            Assert.IsNotNull(SplineContainer, $"SplineContainer == null while performing Play on Follower: {name}");
            
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

        private void Start()
        {
            progressIncrement = Time.fixedDeltaTime / speed;
        }

        private void FixedUpdate () 
        {
            if(!IsPlaying) return;
            
            progress += progressIncrement;
            
            if (progress > 1f)
            {
                IsPlaying = false;
                SetPosition(1f);
                EndOfSplineReached?.Invoke();
                return;
            }
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.localPosition = SplineContainer.EvaluatePosition(progress);
        }
        
        private void SetPosition(float splineProgress)
        {
            transform.localPosition = SplineContainer.EvaluatePosition(splineProgress);
        }
    }
}