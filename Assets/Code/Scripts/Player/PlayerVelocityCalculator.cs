using System;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerVelocityCalculator : MonoBehaviour
    {
        [Header("Curves")] 
        [SerializeField] private AnimationCurve acceleration;
        [SerializeField] private AnimationCurve brake;

        [Header("Debug")] 
        [ReadOnly, SerializeField] private float velocityTime;
        [ReadOnly, SerializeField] private float timeToMaxVelocity, timeToMinVelocity;
        [ReadOnly, SerializeField] private VelocityStates velocityState;

        private float maxVelocity;

        private enum VelocityStates
        {
            None,
            Braking,
            Accelerating,
            Max
        }
        
        public float LastVelocity { get; private set; }

        public int VelocityOrder => (int)velocityState;

        public void StartMovement()
        {
            velocityState = VelocityStates.Accelerating;
            LastVelocity = 0f;
            velocityTime = 0f;
        }

        public void StartBrake()
        {
            velocityState = VelocityStates.Braking;
            velocityTime = 1f - velocityTime * brake[brake.length - 1].time;
        }
        
        public float GetVelocity()
        {
            switch (velocityState)
            {
                case VelocityStates.Max:
                    return maxVelocity;
                case VelocityStates.Braking:
                    LastVelocity = brake.Evaluate(velocityTime);
                    return LastVelocity;
                case VelocityStates.Accelerating:
                    LastVelocity = acceleration.Evaluate(velocityTime);
                    return LastVelocity;
                default:
                    return 0f;
            }
        }

        private void Awake()
        {
            timeToMaxVelocity = acceleration[1].time;
            timeToMinVelocity = brake[1].time;

            maxVelocity = acceleration.Evaluate(timeToMaxVelocity);
        }

        private void OnDisable()
        {
            velocityState = VelocityStates.None;
            LastVelocity = 0f;
            velocityTime = 0f;
        }

        private void Update()
        {
            switch (velocityState)
            {
                case VelocityStates.None:
                    return;
                case VelocityStates.Max:
                    return;
                case VelocityStates.Accelerating:
                    velocityTime += Time.deltaTime;
                    if (velocityTime >= timeToMaxVelocity)
                    {
                        velocityTime = timeToMaxVelocity;
                        velocityState = VelocityStates.Max;
                        LastVelocity = maxVelocity;
                    }
                    break;
                case VelocityStates.Braking:
                    velocityTime += Time.deltaTime;
                    if (velocityTime >= timeToMinVelocity)
                    {
                        velocityState = VelocityStates.None;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}