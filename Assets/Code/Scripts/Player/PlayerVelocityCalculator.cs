using System;
using Meyham.EditorHelpers;
using Meyham.Player.Bodies;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerVelocityCalculator : MonoBehaviour
    {
        private const int max_upper_count = PlayerBody.MAX_NUMBER_OF_BODY_PARTS / 2;

        private const float max_velocity_loss_by_size = 0.5f;

        private const float velocity_loss_by_size =
            max_velocity_loss_by_size / (max_upper_count - PlayerBody.MIN_NUMBER_OF_BODY_PARTS);
        
        [Header("Curves")] 
        [SerializeField] private AnimationCurve acceleration;
        [SerializeField] private AnimationCurve brake;

        [Header("Debug")] 
        [ReadOnly, SerializeField] private float velocityTime;
        [ReadOnly, SerializeField] private float timeToMaxVelocity, timeToMinVelocity;
        [ReadOnly, SerializeField] private VelocityStates velocityState;

        private float maxVelocity, velocityModifier;

        private int bodyCount = PlayerBody.MIN_NUMBER_OF_BODY_PARTS;

        private enum VelocityStates
        {
            None,
            Braking,
            Accelerating,
            Max
        }
        
        [field: ReadOnly, SerializeField]
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
                {
                    LastVelocity = maxVelocity * velocityModifier;
                    break;                    
                }
                case VelocityStates.Braking:
                {
                    LastVelocity = brake.Evaluate(velocityTime);
                    break;
                }
                case VelocityStates.Accelerating:
                {
                    LastVelocity = acceleration.Evaluate(velocityTime) * velocityModifier;
                    break;
                }
            }
            
            return LastVelocity;
        }

        public void OnBodyPartAcquired(BodyPart _)
        {
            bodyCount++;
            CalculateVelocityModifier();
        }

        public void OnBodyPartLost(BodyPart _)
        {
            bodyCount--;
            CalculateVelocityModifier();
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
            bodyCount = 0;
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
                        LastVelocity = 0f;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CalculateVelocityModifier()
        {
            switch (bodyCount)
            {
                case 1:
                {
                    velocityModifier = 1.1f;
                    break;
                }
                case 2:
                {
                    velocityModifier = 1.05f;
                    break;
                }
                case > 3:
                {
                    if (bodyCount >= max_upper_count)
                    {
                        velocityModifier = max_velocity_loss_by_size;
                        break;
                    }

                    velocityModifier = 1f - velocity_loss_by_size * bodyCount;
                    break;
                }
                default:
                {
                    velocityModifier = 1f;
                    break;
                }
            }
        }
    }
}