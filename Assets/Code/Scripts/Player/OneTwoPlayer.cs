using Meyham.Events;
using UnityEngine;

namespace Meyham.Player
{
    /// <summary>
    /// Class used to represent a player in a context of either one or two players
    /// </summary>
    public class OneTwoPlayer : ACollector
    {
        [Header("Input")] 
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;

        [Range(0,5)] public int LeftButton;
        [Range(0,5)] public int RightButton;
        
        [Header("References")] 
        [SerializeField] private RadialPlayerMovement playerMovement;

        private void Start()
        {
            inputEventChannel += OnInputReceived;
        }

        private void OnInputReceived(int input)
        {
            if (input == LeftButton)
            {
                playerMovement.Move(1);
                return;
            }

            if (input != RightButton) return;
            
            playerMovement.Move(-1);
        }

        public override void Collect()
        {
        }
    }
}
