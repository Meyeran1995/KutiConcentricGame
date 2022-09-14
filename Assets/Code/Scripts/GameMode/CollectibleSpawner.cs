using Meyham.DataObjects;
using Meyham.Events;
using Meyham.Items;
using UnityEngine;
using UnityEngine.Pool;

namespace Meyham.GameMode
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VoidEventChannelSO onReleasedEvent;
        
        [Header("Pooling")]
        [SerializeField] private int minPoolSize, maxPoolSize;
        [SerializeField] private GameObject itemTemplate;
        
        private IObjectPool<GameObject> pool;

        private void Awake()
        {
            pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
                true, minPoolSize, maxPoolSize);
        }

        private void Start()
        {
            var preSpawnedObjects = new GameObject[minPoolSize];
            
            for (int i = 0; i < minPoolSize; i++)
            {
                pool.Get(out preSpawnedObjects[i]);
            }
            
            for (int i = 0; i < minPoolSize; i++)
            {
                pool.Release(preSpawnedObjects[i]);
            }
        }

        private GameObject CreatePooledItem()
        {
            var item = Instantiate(itemTemplate);
            item.GetComponentInChildren<ACollectible>().Spawner = this;

            return item;
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
            pool.Release(collectible.transform.parent.gameObject);
            onReleasedEvent.RaiseEvent();
        }

        public void GetCollectible(ItemMovementStatsSO itemStats)
        {
            pool.Get(out var item);
            var movement = item.GetComponent<ItemMovement>();
            movement.transform.position = transform.position;
            movement.SetMovementStats(itemStats);
        }
    }
}
