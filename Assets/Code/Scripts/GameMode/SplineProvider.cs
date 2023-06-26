﻿using UnityEngine;
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
    }
}