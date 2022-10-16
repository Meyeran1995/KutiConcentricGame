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

        private float maxVelocity, lastVelocity;
        
        private enum VelocityStates
        {
            None,
            Accelerating,
            Max,
            Braking
        }

        public void StartMovement()
        {
            velocityState = VelocityStates.Accelerating;
            lastVelocity = 0f;
            velocityTime = 0f;
        }

        public void StartBrake()
        {
            velocityState = VelocityStates.Braking;
            velocityTime = 0f;
        }
        
        public float GetVelocity()
        {
            switch (velocityState)
            {
                case VelocityStates.Max:
                    return maxVelocity;
                case VelocityStates.Braking:
                    return brake.Evaluate(velocityTime);
                case VelocityStates.Accelerating:
                    lastVelocity = acceleration.Evaluate(velocityTime);
                    return lastVelocity;
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

        private void FixedUpdate()
        {
            switch (velocityState)
            {
                case VelocityStates.None:
                    return;
                case VelocityStates.Max:
                    return;
                case VelocityStates.Accelerating:
                    velocityTime += Time.fixedDeltaTime;
                    if (velocityTime >= timeToMaxVelocity)
                    {
                        velocityTime = timeToMaxVelocity;
                        velocityState = VelocityStates.Max;
                        lastVelocity = maxVelocity;
                    }
                    break;
                case VelocityStates.Braking:
                    velocityTime += Time.fixedDeltaTime;
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