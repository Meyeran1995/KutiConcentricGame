using UnityEngine;
using UnityEngine.Pool;

namespace Meyham.GameMode
{
    public abstract class AnObjectPoolBehaviour : MonoBehaviour
    {
        [Header("Pooling")]
        [SerializeField] protected int minPoolSize;
        [SerializeField] protected int maxPoolSize;
        [SerializeField] protected GameObject poolTemplate;

        protected IObjectPool<GameObject> pool;

        protected virtual void Awake()
        {
            pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
                true, minPoolSize, maxPoolSize);
        }
        
        protected virtual void Start()
        {
            var preSpawnedObjects = new GameObject[minPoolSize];
            
            for (int i = 0; i < minPoolSize; i++)
            {
                pool.Get(out preSpawnedObjects[i]);
            }

            foreach (var item in preSpawnedObjects)
            {
                pool.Release(item);
            }
        }

        protected virtual GameObject CreatePooledItem()
        {
            return Instantiate(poolTemplate);
        }

        /// <summary>
        /// Called when an item is returned to the pool using Release
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnReturnedToPool(GameObject item)
        {
            item.SetActive(false);
        }

        /// <summary>
        /// Called when an item is taken from the pool using Get
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnTakeFromPool(GameObject item)
        {
            item.SetActive(true);
        }

        /// <summary>
        /// If the pool capacity is reached then any items returned will be destroyed.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnDestroyPoolObject(GameObject item)
        {
            Destroy(item);
        }
    }
}