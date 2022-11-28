using Meyham.Items;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

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

        public void ReleaseSpline(GameObject collectible)
        {
            var spline = collectible.GetComponent<SplineFollower>().SplineContainer;
            
            spline.Spline.Clear();
            pool.Release(spline.gameObject);
        }

        public SplineContainer GetSpline(BezierKnot[] knots)
        {
            pool.Get(out var splineHolder);
            splineHolder.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
            
            var splineContainer = splineHolder.GetComponent<SplineContainer>();
            var spline = splineContainer.Spline;
            
            foreach (var knot in knots)
            {
                spline.Add(knot);
            }

            return splineContainer;
        }
        
        public SplineContainer GetSpline()
        {
            pool.Get(out var item);
            
            var splineContainer = item.GetComponent<SplineContainer>();

            return splineContainer;
        }

        protected override void Awake()
        {
            base.Awake();
            Random.InitState(System.DateTime.Now.Second);
        }
    }
}