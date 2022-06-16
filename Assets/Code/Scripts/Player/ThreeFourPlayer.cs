using Meyham.Events;
using UnityEngine;

namespace Meyham.Player
{
    public class ThreeFourPlayer : ACollector
    {
        [Header("Button Events")] 
        [SerializeField] private BoolEventChannelSO inputButton;
        [SerializeField] private BoolEventChannelSO middleButton;
        public override void Collect()
        {
        }
    }
}
