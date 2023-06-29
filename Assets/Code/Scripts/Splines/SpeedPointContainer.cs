using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Splines
{
    public class SpeedPointContainer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SplineContainer splineContainer;

        [Header("Parameters")] 
        [SerializeField] private SpeedPoint[] speedPoints;

        private int activeSpeedPoint;

        public bool WasNewSpeedPointReached(float progressOnSpline, out float speedModifier)
        {
            speedModifier = 1f;
            if (activeSpeedPoint == speedPoints.Length) return false;

            if (progressOnSpline < speedPoints[activeSpeedPoint].Progress) return false;

            speedModifier = speedPoints[activeSpeedPoint].Modifier;
            activeSpeedPoint++;
            return true;
        }

        public SpeedPoint[] GetSpeedPoints()
        {
            return speedPoints;
        }
        
        public void SetSpeedPoints(SpeedPoint[] points)
        {
            speedPoints = points;
        }

        private void OnDisable()
        {
            activeSpeedPoint = 0;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (splineContainer != null) return;
            splineContainer = GetComponent<SplineContainer>();
        }

        private void OnDrawGizmos()
        {
            if (speedPoints == null)
            {
                return;
            }

            var position = transform.position;
            
            foreach (var point in speedPoints)
            {
                Vector3 splinePosition = splineContainer.Spline.EvaluatePosition(point.Progress);
                Gizmos.DrawSphere(position + transform.rotation * splinePosition, 0.25f);
            }
        }

#endif
    }
}
