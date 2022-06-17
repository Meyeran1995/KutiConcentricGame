using Meyham.Items;
using UnityEngine;

namespace Meyham.GameMode
{
    public class GameBarrier : MonoBehaviour
    {
        [SerializeField] private CollectibleSpawner spawner;

        private void OnTriggerEnter2D(Collider2D col)
        {
            spawner.ReleaseCollectible(col.GetComponent<ACollectible>());
            col.gameObject.SetActive(false);
        }
    }
}
