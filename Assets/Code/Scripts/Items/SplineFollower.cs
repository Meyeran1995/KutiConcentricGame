using System;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class SplineFollower : MonoBehaviour
    {
        [NonSerialized] public SplineContainer SplineContainer;
        [SerializeField] private FloatValue speed;
        [field: Header("Debug"), ReadOnly, SerializeField] public bool IsPlaying { get; private set; }

        public event Action EndOfSplineReached;
        
        private float progress, progressIncrement;

        public void Restart(bool autoPlay)
        {
            progress = 0f;
            UpdatePosition(0f);
            
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
        
        private void Start()
        {
            progressIncrement = Time.fixedDeltaTime / speed;
        }

        private void FixedUpdate () 
        {
            progress += progressIncrement;
            
            if (progress > 1f)
            {
                IsPlaying = false;
                UpdatePosition(1f);
                EndOfSplineReached?.Invoke();
                return;
            }
            UpdatePosition(progress);
        }

        private void UpdatePosition(float progress)
        {
            transform.localPosition = SplineContainer.EvaluatePosition(progress);
        }
    }
}