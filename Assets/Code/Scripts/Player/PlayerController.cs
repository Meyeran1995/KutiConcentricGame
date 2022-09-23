using UnityEngine;

namespace Meyham.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerInputReceiver input;
        [SerializeField] private PlayerScore score;
        [SerializeField] private Collider playerCollider;

        public PlayerScore Score => score;

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

        public void SetStartingPosition(float angle) => movement.SnapToStartingAngle(angle);

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

#if UNITY_EDITOR

        private void OnValidate()
        {
            playerCollider.isTrigger = true;
        }

#endif
    }
}