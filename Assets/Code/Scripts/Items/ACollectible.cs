using UnityEngine;

namespace Meyham.Items
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ACollectible : MonoBehaviour
    {
        protected abstract void OnCollect();

        private void OnTriggerEnter2D(Collider2D col)
        {
            OnCollect();
            gameObject.SetActive(false);
        }
    }
}
