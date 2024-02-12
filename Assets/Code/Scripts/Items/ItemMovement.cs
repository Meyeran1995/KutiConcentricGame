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
        
        private static CollectiblePool pool;

        private void Awake()
        {
            pool ??= GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectiblePool>();
        }

        public void SetUpMovement(SplineContainer spline, CurveParameter speedChange)
        {
            splineFollower.SetUpSpline(spline, speedChange);
            splineFollower.SetBaseSpeed(speed);
            splineFollower.EndOfSplineReached += OnEndOfSplineReached;
        }

        public void RestartMovement()
        {
            splineFollower.Restart();
        }

        private void OnEndOfSplineReached()
        {
            pool.ReleaseCollectible(gameObject);
        }
    }
}
