using UnityEngine;

namespace Meyham.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject collisionParent;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Header("Player References")]
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerInputReceiver input;
        [SerializeField] private PlayerScore score;
        [SerializeField] private PlayerOrder playerOrder;

        private Color playerColor;
        
        public Color PlayerColor
        {
            get => playerColor;

            set
            {
                spriteRenderer.color = value;
                playerColor = value;
            }
        }

        public PlayerScore Score => score;

        public void SetButton(int button) => input.RightButton = button;

        public void SetStartingPosition(float angle)
        {
            movement.SnapToStartingAngle(angle);
        }

        public void SetPlayerNumber(int number) => score.PlayerNumber = number;

        private void OnEnable()
        {
            movement.enabled = true;
            input.enabled = true;
            playerOrder.enabled = true;
            collisionParent.SetActive(true);
            score.ResetScore();
        }

        private void OnDisable()
        {
            movement.enabled = false;
            input.enabled = false;
            playerOrder.enabled = false;
            collisionParent.SetActive(false);
        }
    }
}