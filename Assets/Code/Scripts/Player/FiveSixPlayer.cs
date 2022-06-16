using Meyham.Events;
using UnityEngine;

namespace Meyham.Player
{
    public class FiveSixPlayer : ACollector
    {
        [Header("Button Events")] 
        [SerializeField] private BoolEventChannelSO inputButton;

        public override void Collect()
        {
        }
    }
}
