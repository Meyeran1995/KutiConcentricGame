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
        [SerializeField] private Collider playerCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public int Order
        {
            get => spriteRenderer.sortingOrder;
            set => spriteRenderer.sortingOrder = value;
        }
        
        public Color PlayerColor { get; private set; }

        public PlayerScore Score => score;
        public RadialPlayerMovement Movement => movement;
        
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

        public void SetButton(int button) => input.RightButton = button;

        public void SetStartingPosition(int index, float angle)
        {
            movement.SnapToStartingAngle(angle);
        }

        public void SetPlayerNumber(int number) => score.PlayerNumber = number;

        public void SetPlayerColor(Color playerColor)
        {
            PlayerColor = playerColor;
            spriteRenderer.color = playerColor;
        }

        public void IncrementOrder() => ChangeOrder(++Order);
        
        public void DecrementOrder() => ChangeOrder(--Order);
        
        public void ChangeOrder(int order)
        {
            Order = order;
            playerOrder.OrderPlayer(order);
        }

        private void OnEnable()
        {
            movement.enabled = true;
            input.enabled = true;
        }

        private void Start()
        {
            PlayerPositionTracker.Register(this);
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