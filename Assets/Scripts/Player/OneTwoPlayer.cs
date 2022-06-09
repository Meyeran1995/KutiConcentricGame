using Meyham.Events;
using UnityEngine;

namespace Meyham.Player
{
    /// <summary>
    /// Class used to represent a player in a context of either one or two players
    /// </summary>
    public class OneTwoPlayer : MonoBehaviour
    {
        [Header("Button Events")] 
        [SerializeField] private BoolEventChannelSO leftButton;
        [SerializeField] private BoolEventChannelSO rightButton;
        
        [Header("References")] 
        [SerializeField] private RadialPlayerMovement playerMovement;

        private void Start()
        {
            leftButton += OnLeftButton;
            rightButton += OnRightButton;
        }

        private void OnLeftButton(bool buttonIsDown)
        {
            playerMovement.Move(1);
        }
        
        private void OnRightButton(bool buttonIsDown)
        {
            playerMovement.Move(0);
        }
    }
}
