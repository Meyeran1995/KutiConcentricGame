using Meyham.DataObjects;
using Meyham.Items;
using UnityEngine;
using UnityEngine.Pool;

namespace Meyham.GameMode
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [SerializeField] private int minPoolSize, maxPoolSize;
        [SerializeField] private GameObject itemTemplate;
        
        private IObjectPool<GameObject> pool;

        private void Awake()
        {
            pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
                true, minPoolSize, maxPoolSize);
        }

        private GameObject CreatePooledItem()
        {
            var item = Instantiate(itemTemplate).GetComponent<ACollectible>();
            item.Spawner = this;
            
            return item.gameObject;
        }

        // Called when an item is returned to the pool using Release
        private void OnReturnedToPool(GameObject item)
        {
            item.SetActive(false);
            item.transform.position = Vector3.zero;
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool(GameObject item)
        {
            item.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        private void OnDestroyPoolObject(GameObject item)
        {
            Destroy(item);
        }
        
        public void ReleaseCollectible(ACollectible collectible)
        {
            Debug.Log($"{collectible.name} has touched the border");
            pool.Release(collectible.gameObject);
        }

        public void GetCollectible(ItemMovementStatsSO itemStats)
        {
            pool.Get(out var item);
            var movement = item.GetComponent<ItemMovement>();
            movement.SetMovementStats(itemStats);
        }
    }
}
