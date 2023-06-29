using Meyham.DataObjects;
using Meyham.GameMode;
using Meyham.Splines;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class ItemMovement : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private FloatParameter speed;
        
        [Header("References")]
        [SerializeField] private SplineFollower splineFollower;
        
        private static CollectibleSpawner spawner;

        private void Awake()
        {
            spawner ??= GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectibleSpawner>();
        }

        public void SetUpMovement(SplineContainer spline, SpeedPoint[] speedPoints)
        {
            splineFollower.SetUpSpline(spline, speedPoints);
            splineFollower.SetBaseSpeed(speed);
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
