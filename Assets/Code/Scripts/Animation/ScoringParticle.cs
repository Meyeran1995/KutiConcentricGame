using Meyham.Player.Bodies;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.Animation
{
    public class ScoringParticle : MonoBehaviour, IPlayerColorReceiver
    {
        [Header("Particles")]
        [SerializeField] private ParticleSystem particleSystemUpper;
        [SerializeField] private ParticleSystem particleSystemLower;

        private bool initialized;

        private static Camera mainCamera;
        
        public void SetColor(int burstCount, Color color)
        {
            var clampedBurst = burstCount;

            if (clampedBurst > PlayerBody.MAX_NUMBER_OF_BODY_PARTS)
            {
                clampedBurst = PlayerBody.MAX_NUMBER_OF_BODY_PARTS / 2;
            }
            else
            {
                clampedBurst /= 2;
            }
            
            var main = particleSystemUpper.main;
            main.startColor = color;
            
            main = particleSystemLower.main;
            main.startColor = color;

            var emission = particleSystemUpper.emission;
            var burst = emission.GetBurst(0);
            burst.count = clampedBurst;
            emission.SetBurst(0, burst);

            emission = particleSystemLower.emission;
            burst = emission.GetBurst(0);
            burst.count = clampedBurst;
            emission.SetBurst(0, burst);
            
            initialized = true;
        }

        private void Start()
        {
            mainCamera ??= Camera.main;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if(!initialized) return;
            
            particleSystemUpper.Play();
            particleSystemLower.Play();
        }

        private void OnDisable()
        {
            initialized = false;
        }
    }
}
