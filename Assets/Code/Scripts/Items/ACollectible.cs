using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Items
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ACollectible : MonoBehaviour
    {
        protected abstract void OnCollect();

        public CollectibleSpawner Spawner;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.CompareTag("Player"))
                OnCollect();
            Spawner.ReleaseCollectible(this);
        }
    }
}
