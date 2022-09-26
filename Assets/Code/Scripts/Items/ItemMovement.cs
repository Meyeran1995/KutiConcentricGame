using Meyham.GameMode;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class ItemMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SplineFollower splineFollower;
        
        public SplineContainer Spline => splineFollower.SplineContainer;

        private static CollectibleSpawner spawner;

        private void Awake()
        {
            if(spawner) return;

            spawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectibleSpawner>();
        }

        public void SetSpline(SplineContainer spline)
        {
            splineFollower.SplineContainer = spline;
            splineFollower.EndOfSplineReached += OnEndOfSplineReached;
        }

        public void RestartMovement()
        {
            splineFollower.Restart(true);
        }

        private void OnEndOfSplineReached()
        {
            spawner.ReleaseCollectible(gameObject);
        }
    }
}
