using System.Collections;
using Meyham.EditorHelpers;
using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Items
{
    [RequireComponent(typeof(Collider))]
    public class ItemCollision : MonoBehaviour
    {
        [SerializeField] private Collider itemCollider;
        [Space]
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private ItemTweeningAnimation tweenAnimation;

        [Header("Collectibles")]
        [SerializeField] private ACollectible score;
        [SerializeField] private PowerUp powerUp;

        [field: SerializeField, ReadOnly]
        public bool HasPowerUp { get; private set; }

        private static CollectibleSpawner spawner;

        public void SetPowerUpEffect(APowerUpEffect effect)
        {
            powerUp.Effect = effect;
            HasPowerUp = true;
        }

        public void RemovePowerUpEffect()
        {
            powerUp.Effect = null;
            HasPowerUp = false;
        }
        
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
            
            if (!HasPowerUp) return;
            
            powerUp.Collect(receiver);
        }

        private IEnumerator WaitForShrinkAnimation()
        {
            yield return tweenAnimation.TweenShrink();
            
            spawner.ReleaseCollectible(transform.parent.gameObject);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            HasPowerUp = powerUp != null && powerUp.Effect != null;
        }

#endif
    }
}