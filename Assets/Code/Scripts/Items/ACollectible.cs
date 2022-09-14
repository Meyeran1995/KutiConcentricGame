using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Items
{
    [RequireComponent(typeof(Collider))]
    public abstract class ACollectible : MonoBehaviour
    {
        public CollectibleSpawner Spawner;

        protected abstract void OnCollect(GameObject player);
        
        private void OnTriggerEnter(Collider other)
        {
            GameObject incomingObject = other.gameObject;

            if (incomingObject.CompareTag("Player"))
            {
                OnCollect(incomingObject);
            }
            
            Spawner.ReleaseCollectible(this);
        }

        private void OnBecameInvisible()
        {
            Spawner.ReleaseCollectible(this);
        }
    }
}
