using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RadialPlayerMovement movement;
        [SerializeField] private PlayerInputReceiver input;
        [SerializeField] private PlayerScore score;
        [SerializeField] private PlayerOrder playerOrder;
        [SerializeField] private Collider itemCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public Color PlayerColor { get; private set; }

        public PlayerScore Score => score;
        public RadialPlayerMovement Movement => movement;
        
        public void OnGameEnd()
        {
            enabled = false;
        }

        public void OnGameRestart()
        {
            enabled = true;
            score.ResetScore();
        }

        public void SetButton(int button) => input.RightButton = button;

        public void SetStartingPosition(float angle)
        {
            movement.SnapToStartingAngle(angle);
        }

        public void SetPlayerNumber(int number) => score.PlayerNumber = number;

        public void SetPlayerColor(Color playerColor)
        {
            PlayerColor = playerColor;
            spriteRenderer.color = playerColor;
        }

        public void IncrementOrder() => ChangeOrder(1);
        
        public void DecrementOrder() => ChangeOrder(0);
        
        public void ChangeOrder(int order)
        {
            playerOrder.OrderPlayer(order);
        }

        private void OnEnable()
        {
            movement.enabled = true;
            input.enabled = true;
            itemCollider.transform.parent.gameObject.SetActive(true);
            // itemCollider.isTrigger = true;
            // playerCollider.isTrigger = true;
        }

        private void Start()
        {
            PlayerPositionTracker.Register(this);
        }

        private void OnDisable()
        {
            movement.enabled = false;
            input.enabled = false;
            // itemCollider.isTrigger = false;
            // playerCollider.isTrigger = false;
            itemCollider.transform.parent.gameObject.SetActive(false);
        }
    }
}