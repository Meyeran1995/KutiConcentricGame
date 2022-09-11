using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Items
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ACollectible : MonoBehaviour
    {
        public CollectibleSpawner Spawner;

        protected abstract void OnCollect(GameObject player);
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject incomingObject = col.gameObject;
            
            if(incomingObject.CompareTag("Player"))
                OnCollect(incomingObject);
            Spawner.ReleaseCollectible(this);
        }
    }
}
