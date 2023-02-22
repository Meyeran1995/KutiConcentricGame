using Meyham.Events;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerInputReceiver : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;
        [SerializeField] private GenericEventChannelSO<int> inputCanceledEventChannel;
        
        [Header("References")] 
        [SerializeField] private RadialPlayerMovement playerMovement;
        [SerializeField] private PlayerController controller;

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
            if (input != (int)controller.Designation) return;
            playerMovement.StartMovement();
        }
        
        private void OnInputCanceled(int input)
        {
            if (input != (int)controller.Designation) return;
            playerMovement.Brake();
        }
    }
}
