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
        
        public void SetColor(int _, Color color)
        {
            var main = particleSystemUpper.main;
            main.startColor = color;
            
            main = particleSystemLower.main;
            main.startColor = color;
            
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
