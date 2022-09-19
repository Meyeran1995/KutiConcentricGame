using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Items
{
    [RequireComponent(typeof(Collider))]
    public abstract class ACollectible : MonoBehaviour
    {
        private static CollectibleSpawner spawner;

        private void Awake()
        {
            if (spawner) return;

            spawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<CollectibleSpawner>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var incomingObject = other.gameObject;

            if (incomingObject.CompareTag("Player")) OnCollect(incomingObject);

            spawner.ReleaseCollectible(this);
        }

        protected abstract void OnCollect(GameObject player);
    }
}