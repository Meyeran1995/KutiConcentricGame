using Meyham.Events;
using Meyham.Items;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.GameMode
{
    public class CollectibleSpawner : AnObjectPoolBehaviour
    {
        [Header("References")]
        [SerializeField] private VoidEventChannelSO onReleasedEvent;
        [SerializeField] private SplineProvider splineProvider;

        public void ReleaseCollectible(GameObject collectible)
        {
            splineProvider.ReleaseSpline(collectible);
            pool.Release(collectible);
            onReleasedEvent.RaiseEvent();
        }
        
        public void ReleaseCollectible(ACollectible collectible)
        {
            ReleaseCollectible(collectible.transform.parent.gameObject);
        }

        public void GetCollectible(BezierKnot[] splineKnots)
        {
            pool.Get(out var item);
            var movement = item.GetComponent<ItemMovement>();
            movement.SetSpline(splineProvider.GetSpline(splineKnots));
            movement.RestartMovement();
        }
    }
}
