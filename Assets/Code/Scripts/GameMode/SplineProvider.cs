using Meyham.Splines;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.GameMode
{
    public class SplineProvider : AnObjectPoolBehaviour
    {
        protected override GameObject CreatePooledItem()
        {
            var spline = Instantiate(poolTemplate);
            spline.transform.position = transform.position;

            return spline;
        }

        public void ReleaseSpline(SplineContainer collectibleSpline)
        {
            collectibleSpline.Spline.Clear();
            pool.Release(collectibleSpline.gameObject);
        }

        public SplineContainer GetSpline(SplineKnotData splineData)
        {
            pool.Get(out var splineHolder);
            splineHolder.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
            
            var splineContainer = splineHolder.GetComponent<SplineContainer>();
            var spline = splineContainer.Spline;

            for (var i = 0; i < splineData.Knots.Length; i++)
            {
                var knot = splineData.Knots[i];
                spline.Add(knot);
                spline.SetTangentMode(i, splineData.TangentModes[i]);
            }

            return splineContainer;
        }
    }
}