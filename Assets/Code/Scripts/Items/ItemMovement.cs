using Meyham.DataObjects;
using Meyham.GameMode;
using UnityEngine;
using UnityEngine.Assertions;
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
            splineFollower.Speed = speed;
        }

        public void SetSpline(SplineContainer spline)
        {
            Assert.IsNotNull(spline, "spline == null");
            
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
        
#if UNITY_EDITOR
        
        //Keep updating in case of speed value being modified
        private void Update()
        {
            splineFollower.Speed = speed;
        }
        
#endif
    }
}
