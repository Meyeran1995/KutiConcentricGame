using Meyham.EditorHelpers;
using Meyham.Events;
using UnityEngine;

namespace Meyham.Player
{
    /// <summary>
    /// Class that receives and evaluates Input Events for a single player
    /// </summary>
    public class PlayerInputReceiver : MonoBehaviour
    {
        [Header("Input")] 
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;

        [Range(-1,5)] public int LeftButton;
        [Range(-1,5)] public int RightButton;

        [ReadOnly, SerializeField] private int leftMovement, rightMovement;

        [Header("References")] 
        [SerializeField] private RadialPlayerMovement playerMovement;

        public void FlipMovementDirection()
        {
            leftMovement = rightMovement;
            rightMovement = -rightMovement;
        }

        private void Awake()
        {
            leftMovement = 1;
            rightMovement = -leftMovement;
        }

        private void OnEnable()
        {
            inputEventChannel += OnInputReceived;
        }
        
        private void OnDisable()
        {
            inputEventChannel -= OnInputReceived;
        }

        private void OnInputReceived(int input)
        {
            if (input == LeftButton)
            {
                playerMovement.Move(leftMovement);
                return;
            }

            if (input != RightButton) return;
            
            playerMovement.Move(rightMovement);
        }
    }
}