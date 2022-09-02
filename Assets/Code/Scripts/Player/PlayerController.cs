using UnityEngine;

namespace Meyham.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerInputReceiver input;

        public void SetLeftButton(int button)
        {
            input.LeftButton = button;
        }
        
        public void SetRightButton(int button)
        {
            input.RightButton = button;
        }

        public void SetStartingPosition(float angle)
        {
            movement.StartingAngle = angle;
        }
        
        private void OnEnable()
        {
            movement.enabled = true;
            input.enabled = true;
        }
        
        private void OnDisable()
        {
            movement.enabled = false;
            input.enabled = false;
        }
    }
}