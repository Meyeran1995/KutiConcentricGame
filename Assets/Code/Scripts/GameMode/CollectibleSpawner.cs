using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.Events;
using Meyham.Items;
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
            public readonly ItemSpriteController SpriteController;
            
            public bool HasPowerUp => collision.HasPowerUp;

            private readonly ItemCollision collision;
            
            public CollectibleReferenceCache(ItemCollision collision,
                ItemMovement movement,
                ItemSpriteController spriteController)
            {
                this.collision = collision;
                
                Movement = movement;
                SpriteController = spriteController;
            }

            public void AddPowerUp(APowerUpEffect powerUpEffect)
            {
                collision.SetPowerUpEffect(powerUpEffect);
            }
            
            public void RemovePowerUp()
            {
                collision.RemovePowerUpEffect();
            }
        }
        
        public void GetCollectible(ItemData itemData)
        {
            pool.Get(out var item);
            var cache = referenceCaches[item];

            cache.SpriteController.SetSprite(itemData.Sprite);

            var movement = cache.Movement;
            movement.SetSpline(splineProvider.GetSpline(itemData.MovementData));
            movement.RestartMovement();

            item.SetActive(true);
            
            if (!itemData.HasPowerUp) return;
            
            cache.AddPowerUp(itemData.PowerUpData);
        }
        
        public void ReleaseCollectible(GameObject collectible)
        {
            splineProvider.ReleaseSpline(collectible);
            pool.Release(collectible);
            onReleasedEvent.RaiseEvent();
        }
        
        protected override GameObject CreatePooledItem()
        {
            var item = Instantiate(poolTemplate);
            var cache = new CollectibleReferenceCache(item.GetComponentInChildren<ItemCollision>(),
                item.GetComponent<ItemMovement>(), item.GetComponent<ItemSpriteController>());
            
            referenceCaches.Add(item, cache);
            
            return item;
        }

        protected override void OnReturnedToPool(GameObject item)
        {
            item.SetActive(false);

            var cache = referenceCaches[item];
            
            if (!cache.HasPowerUp) return;
            
            cache.RemovePowerUp();
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
