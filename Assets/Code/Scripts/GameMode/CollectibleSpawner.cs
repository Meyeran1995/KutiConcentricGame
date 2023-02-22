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

        private struct CollectibleReferenceCache
        {
            public readonly ItemMovement Movement;
            public readonly ItemSpriteController SpriteController;
            public PowerUp PowerUp { get; private set; }
            public bool HasPowerUp { get; private set; }

            private readonly ItemCollision collision;
            
            public CollectibleReferenceCache(ItemCollision collision,
                ItemMovement movement,
                ItemSpriteController spriteController)
            {
                this.collision = collision;
                
                Movement = movement;
                SpriteController = spriteController;
                PowerUp = null;
                HasPowerUp = false;
            }

            public void AddPowerUp(PowerUp powerUp)
            {
                collision.SetPowerUpCollectible(powerUp);
                PowerUp = powerUp;
                HasPowerUp = true;
            }
            
            public void RemovePowerUp()
            {
                collision.SetPowerUpCollectible(null);
                PowerUp = null;
                HasPowerUp = false;
            }
        }

        private static readonly Dictionary<GameObject, CollectibleReferenceCache> ReferenceCache = new();
        
        public void GetCollectible(ItemData itemData)
        {
            pool.Get(out var item);
            var cache = ReferenceCache[item];

            cache.SpriteController.SetSprite(itemData.Sprite);

            var movement = cache.Movement;
            movement.SetSpline(splineProvider.GetSpline(itemData.MovementData));
            movement.RestartMovement();

            if (itemData.IsPowerUp)
            {
                var powerUp = item.transform.GetChild(3).gameObject.AddComponent<PowerUp>();
                powerUp.Effect = itemData.PowerUpData;
                cache.AddPowerUp(powerUp);
            }
            
            item.SetActive(true);
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
            
            ReferenceCache.Add(item, cache);
            
            return item;
        }

        protected override void OnReturnedToPool(GameObject item)
        {
            item.SetActive(false);

            var cache = ReferenceCache[item];
            
            if (!cache.HasPowerUp) return;
            
            Destroy(cache.PowerUp);
            cache.RemovePowerUp();
        }

        protected override void OnDestroyPoolObject(GameObject item)
        {
            ReferenceCache.Remove(item);
            Destroy(item);
        }
    }
}
