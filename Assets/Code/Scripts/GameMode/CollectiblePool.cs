using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.Events;
using Meyham.Items;
using Meyham.Splines;
using UnityEngine;

namespace Meyham.GameMode
{
    public class CollectiblePool : AnObjectPoolBehaviour
    {
        [Header("References")]
        [SerializeField] private VoidEventChannelSO onReleasedEvent;
        [SerializeField] private SplineProvider splineProvider;
        [SerializeField] private Material takeBodyPartMaterial;

        private Material addBodyPartMaterial;

        private Dictionary<GameObject, CollectibleReferenceCache> referenceCaches;

        private readonly struct CollectibleReferenceCache
        {
            public readonly ItemMovement Movement;
            public readonly SplineFollower SplineFollower;
            public readonly ItemSpriteController SpriteController;
            public readonly ItemRotator ItemRotator;
            public readonly ItemCollision ItemCollision;
            public readonly ItemCollectibleCarrier CollectibleCarrier;
            
            public CollectibleReferenceCache(ItemMovement movement, SplineFollower splineFollower,
                ItemSpriteController spriteController, ItemRotator itemRotator, ItemCollision itemCollision, ItemCollectibleCarrier collectibleCarrier)
            {
                Movement = movement;
                SplineFollower = splineFollower;
                SpriteController = spriteController;
                ItemRotator = itemRotator;
                ItemCollision = itemCollision;
                CollectibleCarrier = collectibleCarrier;
            }
        }
        
        public void GetCollectible(ItemData itemData)
        {
            pool.Get(out var item);
            
            var cache = referenceCaches[item];
            var collectible = itemData.CollectibleData;
            var isTakeBodyPartCollectible = collectible is DestroyBodyPartCollectible;
            
            cache.CollectibleCarrier.SetCollectible(collectible);
            cache.ItemRotator.enabled = isTakeBodyPartCollectible;
            
            var spriteController = cache.SpriteController;
            spriteController.SetSprite(itemData.Sprite);
            spriteController.SetMaterial(isTakeBodyPartCollectible ? takeBodyPartMaterial : addBodyPartMaterial);
            spriteController.SetColor(itemData.Color);
            
            cache.ItemCollision.ReceiveColliderDimensions(itemData.ColliderPosition, 
                itemData.ColliderRotation, itemData.ColliderScale);

            var movement = cache.Movement;
            movement.SetUpMovement(splineProvider.GetSpline(itemData.MovementData), itemData.SpeedCurve);
            movement.RestartMovement();
            
            item.SetActive(true);
        }
        
        public void ReleaseCollectible(GameObject collectible)
        {
            splineProvider.ReleaseSpline(referenceCaches[collectible].SplineFollower.GetTargetSpline());
            pool.Release(collectible);
            onReleasedEvent.RaiseEvent();
        }

        public void SetPlayerCountSpecificMaterial(Material material)
        {
            addBodyPartMaterial = material;
        }
        
        protected override GameObject CreatePooledItem()
        {
            var item = Instantiate(poolTemplate);
            var itemCollision = item.GetComponentInChildren<ItemCollision>();
            var cache = new CollectibleReferenceCache(item.GetComponent<ItemMovement>(),
                item.GetComponent<SplineFollower>(),
                item.GetComponentInChildren<ItemSpriteController>(), 
                item.GetComponent<ItemRotator>(),
                itemCollision,
                itemCollision.GetComponent<ItemCollectibleCarrier>()
                );
            
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
