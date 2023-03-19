using UnityEngine;

namespace Meyham.Collision
{
    public class PlayerColliderUpdater : MonoBehaviour
    {
        private static Vector3 startingScale;

        public void ModifyCollisionSize(float sizeFactor, int order)
        {
            if (order == 0)
            {
                transform.localScale = startingScale;
                return;
            }

            float scalePercentage = 1f - sizeFactor * order;
            Vector3 newScale = startingScale;
            newScale.x *= scalePercentage;
            
            transform.localScale = newScale;
        }

        private void Awake()
        {
            if(startingScale != Vector3.zero) return;
            
            startingScale = transform.localScale;
        }
    }
}
