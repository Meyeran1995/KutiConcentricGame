using System;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class SplineFollower : MonoBehaviour
    {
        [HideInInspector] public float Speed;
        [field: Header("Debug"), ReadOnly, SerializeField] public bool IsPlaying { get; private set; }
        [ReadOnly] public SplineContainer SplineContainer;

        public event Action EndOfSplineReached;

        [SerializeField, ReadOnly] private float progress;
        
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

        private void Update() 
        {
            if(!IsPlaying) return;
            
            progress += Time.deltaTime / Speed;
            
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
            transform.localPosition = SplineContainer.EvaluatePosition(progress);
        }
        
        private void SetPosition(float splineProgress)
        {
            transform.localPosition = SplineContainer.EvaluatePosition(splineProgress);
        }
    }
}