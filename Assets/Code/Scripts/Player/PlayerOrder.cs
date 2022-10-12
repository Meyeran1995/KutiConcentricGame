using System;
using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerOrder : MonoBehaviour
    {
        [SerializeField] private FloatValue sizeFactor;

        private Vector3 startingScale;
        
        public void OrderPlayer(int order)
        {
            if (order == 0)
            {
                transform.localScale = startingScale;
                return;
            }

            float scale = sizeFactor * order;
            
            transform.localScale = startingScale - new Vector3(scale, scale, 0f);
        }

        private void Awake()
        {
            startingScale = transform.localScale;
        }
    }
}