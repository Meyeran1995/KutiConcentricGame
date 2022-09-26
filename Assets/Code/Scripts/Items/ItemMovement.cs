using Meyham.DataObjects;
using Meyham.GameMode;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class ItemMovement : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private FloatValue speed;
        
        [Header("References")]
        [SerializeField] private SplineAnimate splineFollower;
        
        public SplineContainer Spline => splineFollower.splineContainer;

        private static CollectibleSpawner spawner;

        private void Awake()
        {
            splineFollower.maxSpeed = speed.BaseValue;
            
            if(spawner) return;

            spawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectibleSpawner>();
        }

        public void SetSpline(SplineContainer spline)
        {
            splineFollower.splineContainer = spline;
        }

        public void RestartMovement()
        {
            splineFollower.Restart(true);
        }
        
        private void FixedUpdate()
        {
            if(splineFollower.isPlaying) return;
            
            spawner.ReleaseCollectible(gameObject);
        }
    }
}
