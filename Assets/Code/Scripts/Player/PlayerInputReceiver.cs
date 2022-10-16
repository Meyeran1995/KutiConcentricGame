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
        [Range(0,6)] public int RightButton;
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel, inputCanceledEventChannel;

        [Header("References")] 
        [SerializeField] private RadialPlayerMovement playerMovement;

        private int movementDirection;

        public void FlipMovementDirection()
        {
            movementDirection = -movementDirection;
            playerMovement.movementDirection = movementDirection;
            //TODO: do not flip direction when braking
        }

        private void Awake()
        {
            movementDirection = -1;
        }

        private void OnEnable()
        {
            inputEventChannel += OnInputReceived;
            inputCanceledEventChannel += OnInputCanceled;
        }
        
        private void OnDisable()
        {
            inputEventChannel -= OnInputReceived;
            inputCanceledEventChannel -= OnInputCanceled;
        }

        private void OnInputReceived(int input)
        {
            if (input != RightButton) return;
            playerMovement.movementDirection = movementDirection;
            playerMovement.StartMovement();
        }
        
        private void OnInputCanceled(int input)
        {
            if (input != RightButton) return;
            playerMovement.Brake();
        }
    }
}
