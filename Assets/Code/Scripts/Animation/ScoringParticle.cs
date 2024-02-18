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

        public void SetWorldPosition(RectTransform linkedEntry)
        {
            var worldCorners = new Vector3[4];
            linkedEntry.GetWorldCorners(worldCorners);
            
            var position = new Vector3(Mathf.Lerp(worldCorners[0].x, worldCorners[3].x, 0.5f),Mathf.Lerp(worldCorners[0].y, worldCorners[1].y, 0.5f));
            position.z = 4.8f;
            transform.position = mainCamera.ScreenToWorldPoint(position);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            mainCamera ??= Camera.main;
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
