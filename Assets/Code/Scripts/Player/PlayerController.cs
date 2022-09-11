using UnityEngine;

namespace Meyham.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerInputReceiver input;
        [SerializeField] private PlayerScore score;
        [SerializeField] private Collider2D playerCollider;

        public void OnGameEnd()
        {
            playerCollider.isTrigger = false;
            enabled = false;
        }

        public void OnGameRestart()
        {
            playerCollider.isTrigger = true;
            enabled = true;
            score.ResetScore();
        }

        public void SetLeftButton(int button) => input.LeftButton = button;

        public void SetRightButton(int button) => input.RightButton = button;

        public void SetStartingPosition(float angle) => movement.StartingAngle = angle;

        public void SetPlayerNumber(int number) => score.PlayerNumber = number;

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