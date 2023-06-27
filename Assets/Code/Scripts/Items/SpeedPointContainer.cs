using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class SpeedPointContainer : MonoBehaviour
    {
        // define speed points based on spline progress (0-1)
        // provide method to set speed upon reaching a speed point?
        [Header("References")]
        [SerializeField] private SplineContainer splineContainer;
        [Header("Parameters")]
        [SerializeField] private float[] progressPerPoint;
        [SerializeField] private float[] speedModifierPerPoint;

        private int activeSpeedPoint;

        public bool WasNewSpeedPointReached(float progressOnSpline, out float speedModifier)
        {
            speedModifier = 1f;
            if (activeSpeedPoint == progressPerPoint.Length) return false;

            if (progressOnSpline < progressPerPoint[activeSpeedPoint]) return false;

            speedModifier = speedModifierPerPoint[activeSpeedPoint];
            activeSpeedPoint++;
            return true;
        }
        
        private void OnDisable()
        {
            activeSpeedPoint = 0;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (splineContainer == null)
            {
                splineContainer = GetComponent<SplineContainer>();
            }
            
            if (progressPerPoint == null || speedModifierPerPoint == null)
            {
                return;
            }

            for (int i = 0; i < progressPerPoint.Length; i++)
            {
                progressPerPoint[i] = Mathf.Clamp01(progressPerPoint[i]);
            }
        }

        private void OnDrawGizmos()
        {
            if (progressPerPoint == null || speedModifierPerPoint == null)
            {
                return;
            }

            var position = transform.position;
            
            foreach (var point in progressPerPoint)
            {
                Vector3 splinePosition = splineContainer.Spline.EvaluatePosition(point);
                Gizmos.DrawSphere(position + transform.rotation * splinePosition, 0.25f);
            }
        }

#endif
    }
}
