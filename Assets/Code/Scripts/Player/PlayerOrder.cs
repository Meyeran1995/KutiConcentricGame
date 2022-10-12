using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerOrder : MonoBehaviour
    {
        [SerializeField] private FloatValue sizeFactor;

        private static Vector3 startingScale;
        
        public void OrderPlayer(int order)
        {
            if (order == 0)
            {
                transform.localScale = startingScale;
                return;
            }

            float scalePercentage = 1f - sizeFactor * order;
            Vector3 newScale = startingScale;
            newScale.x *= scalePercentage;
            newScale.y *= scalePercentage;
            
            transform.localScale = newScale;
        }

        private void Awake()
        {
            if(startingScale != Vector3.zero) return;
            
            startingScale = transform.localScale;
        }
    }
}