using Meyham.DataObjects;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.Items
{
    public class SplineFollower : MonoBehaviour
    {
        [SerializeField] private SplineContainer spline;
        [SerializeField] private FloatValue speed;

        private float progress;

        private void FixedUpdate () {
            progress += Time.deltaTime / speed;
            if (progress > 1f)
            {
                progress = 1f;
            }
            transform.localPosition = spline.EvaluatePosition(progress);
        }
    }
}