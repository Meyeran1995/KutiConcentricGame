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
            public readonly AddScoreCollectible AddScoreCollectible;
            public readonly ItemMovement Movement;
            public readonly SpriteRenderer Renderer;
            public PowerUp PowerUp { get; private set; }
            public bool HasPowerUp { get; private set; }

            public CollectibleReferenceCache(AddScoreCollectible addScoreCollectible, ItemMovement movement, SpriteRenderer renderer)
            {
                AddScoreCollectible = addScoreCollectible;
                Movement = movement;
                Renderer = renderer;
                PowerUp = null;
                HasPowerUp = false;
            }

            public void AddPowerUp(PowerUp powerUp)
            {
                PowerUp = powerUp;
                HasPowerUp = true;
            }
            
            public void RemovePowerUp()
            {
                PowerUp = null;
                HasPowerUp = false;
            }
        }

        private static readonly Dictionary<GameObject, CollectibleReferenceCache> ReferenceCache = new();
        
        public void GetCollectible(ItemData itemData)
        {
            pool.Get(out var item);
            var cache = ReferenceCache[item];

            cache.AddScoreCollectible.Score = itemData.ScoreData;
            cache.Renderer.color = itemData.Color;

            var movement = cache.Movement;
            movement.SetSpline(splineProvider.GetSpline(itemData.MovementData));
            movement.RestartMovement();

            if (!itemData.IsPowerUp) return;
            
            var powerUp = item.transform.GetChild(0).gameObject.AddComponent<PowerUp>();
            powerUp.Effect = itemData.PowerUpData;
            cache.AddPowerUp(powerUp);
        }
        
        public void ReleaseCollectible(GameObject collectible)
        {
            if(!collectible.activeSelf) return; // hack against invalid operation exception, when collectibles somehow end up releasing twice
            
            splineProvider.ReleaseSpline(collectible);
            pool.Release(collectible);
            onReleasedEvent.RaiseEvent();
        }
        
        protected override GameObject CreatePooledItem()
        {
            var item = Instantiate(poolTemplate);
            var cache = new CollectibleReferenceCache(item.GetComponentInChildren<AddScoreCollectible>(),
                item.GetComponent<ItemMovement>(), item.transform.GetChild(2).GetComponent<SpriteRenderer>());
            
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
