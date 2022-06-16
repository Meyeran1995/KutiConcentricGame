using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.GameMode
{
    public class EndTimer : MonoBehaviour
    {
        [SerializeField] private FloatValue startingTime;

        private float currentTime;

        private void Awake()
        {
            currentTime = startingTime;
        }
    }
}