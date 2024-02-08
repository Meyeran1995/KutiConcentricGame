using System.Collections;
using Meyham.Animation;
using Meyham.DataObjects;
using Meyham.GameMode;
using Meyham.Player.Bodies;
using Unity.Collections;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemCollectibleCarrier : MonoBehaviour
    {
        [SerializeField] private ItemTweeningAnimation tweenAnimation;
        
        [field: ReadOnly, SerializeField] 
        public ACollectibleData Collectible { get; private set; }
        
        private static CollectiblePool pool;
        
        public void SetCollectible(ACollectibleData collectibleData)
        {
            Collectible = collectibleData;
            enabled = true;
        }

        public void OnCollected(GameObject playerBody, GameObject part)
        {
            enabled = false;

            ATweenBasedAnimation animationHandle;
            if (Collectible is AddBodyPartCollectible)
            {
                animationHandle = new AddBodyCollectionAnimationHandle(tweenAnimation);
                Collectible.Collect(playerBody, animationHandle);
                StartCoroutine(WaitForCollectionAnimation(animationHandle));
                return;
            }

            animationHandle = new DestroyBodyCollectionHandle(part.GetComponentInParent<BodyPart>());
            Collectible.Collect(playerBody, animationHandle);
            StartCoroutine(WaitForShrinkAnimation());
        }
        
        private void Awake()
        {
            pool ??= GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectiblePool>();
        }
        
        private IEnumerator WaitForShrinkAnimation()
        {
            yield return tweenAnimation.TweenShrink();
            
            pool.ReleaseCollectible(transform.parent.gameObject);
        }
        
        private IEnumerator WaitForCollectionAnimation(ATweenBasedAnimation handle)
        {
            yield return handle;

            var parent = transform.parent;
            parent.SetParent(null);
            
            pool.ReleaseCollectible(parent.gameObject);
        }
    }
}