using System.Collections;
using Meyham.EditorHelpers;
using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Items
{
    [RequireComponent(typeof(Collider))]
    public class ItemCollision : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Collider itemCollider;
        [Space]
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private ItemTweeningAnimation tweenAnimation;
        [Space]
        [SerializeField] private ACollectible score;

        private static CollectibleSpawner spawner;

        private void Awake()
        {
            spawner ??= GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectibleSpawner>();
        }
        
        private void OnEnable()
        {
            itemCollider.isTrigger = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var incomingObject = other.gameObject;

            if (!incomingObject.CompareTag("Player")) return;

            itemCollider.isTrigger = false;
            splineFollower.Pause();
            
            DistributeCollectibles(incomingObject);
            StartCoroutine(WaitForShrinkAnimation());
        }

        private void DistributeCollectibles(GameObject receiver)
        {
            score.Collect(receiver);
        }

        private IEnumerator WaitForShrinkAnimation()
        {
            yield return tweenAnimation.TweenShrink();
            
            spawner.ReleaseCollectible(transform.parent.gameObject);
        }
    }
}