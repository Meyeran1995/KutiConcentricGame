using System.Collections.Generic;
using Meyham.Player.Bodies;
using UnityEngine;

namespace Meyham.GameMode
{
    public class PlayerBodyPartPool : AnObjectPoolBehaviour
    {
        private static readonly Dictionary<GameObject, BodyPart> BodyPartByItem = new(18);

        private CollectiblePool collectiblePool;

        public BodyPart GetBodyPart()
        {
            pool.Get(out var part);

            return BodyPartByItem[part];
        }
        
        public void ReleaseBodyPart(GameObject bodyPart)
        {
            pool.Release(bodyPart);
        }
        
        protected override GameObject CreatePooledItem()
        {
            var item = base.CreatePooledItem();
            BodyPartByItem.Add(item, item.GetComponent<BodyPart>());

            return item;
        }

        protected override void OnReturnedToPool(GameObject item)
        {
            if (item.transform.childCount > 1)
            {
                collectiblePool ??= GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectiblePool>();
                var collectibleTransform = item.transform.GetChild(1);
                collectibleTransform.parent = null;
                collectiblePool.ReleaseCollectible(collectibleTransform.gameObject);
            }
            
            base.OnReturnedToPool(item);
            item.transform.parent = null;
        }

        protected override void OnDestroyPoolObject(GameObject item)
        {
            BodyPartByItem.Remove(item);
            base.OnDestroyPoolObject(item);
        }
    }
}