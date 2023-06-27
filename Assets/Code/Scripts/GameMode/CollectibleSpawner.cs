using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.Events;
using Meyham.Items;
using Meyham.Splines;
using UnityEngine;

namespace Meyham.GameMode
{
    public class CollectibleSpawner : AnObjectPoolBehaviour
    {
        [Header("References")]
        [SerializeField] private VoidEventChannelSO onReleasedEvent;
        [SerializeField] private SplineProvider splineProvider;

        private Dictionary<GameObject, CollectibleReferenceCache> referenceCaches;

        private readonly struct CollectibleReferenceCache
        {
            public readonly ItemMovement Movement;
            public readonly SplineFollower SplineFollower;
            public readonly ItemSpriteController SpriteController;
            public readonly ItemCollision ItemCollision;
            
            public CollectibleReferenceCache(ItemMovement movement, SplineFollower splineFollower,
                ItemSpriteController spriteController, ItemCollision itemCollision)
            {
                Movement = movement;
                SplineFollower = splineFollower;
                SpriteController = spriteController;
                ItemCollision = itemCollision;
            }
        }
        
        public void GetCollectible(ItemData itemData)
        {
            pool.Get(out var item);
            var cache = referenceCaches[item];

            cache.SpriteController.SetSprite(itemData.Sprite);
            
            cache.ItemCollision.ReceiveColliderDimensions(itemData.ColliderPosition, 
                itemData.ColliderRotation, itemData.ColliderScale);

            var movement = cache.Movement;
            movement.SetSpline(splineProvider.GetSpline(itemData.MovementData));
            movement.RestartMovement();

            item.SetActive(true);
        }
        
        public void ReleaseCollectible(GameObject collectible)
        {
            splineProvider.ReleaseSpline(referenceCaches[collectible].SplineFollower.GetTargetSpline());
            pool.Release(collectible);
            onReleasedEvent.RaiseEvent();
        }
        
        protected override GameObject CreatePooledItem()
        {
            var item = Instantiate(poolTemplate);
            var cache = new CollectibleReferenceCache(item.GetComponent<ItemMovement>(),
                item.GetComponent<SplineFollower>(),
                item.GetComponent<ItemSpriteController>(), 
                item.GetComponentInChildren<ItemCollision>());
            
            referenceCaches.Add(item, cache);
            
            return item;
        }

        protected override void OnDestroyPoolObject(GameObject item)
        {
            referenceCaches.Remove(item);
            Destroy(item);
        }

        protected override void Awake()
        {
            referenceCaches = new Dictionary<GameObject, CollectibleReferenceCache>(minPoolSize);
            base.Awake();
        }
    }
}
