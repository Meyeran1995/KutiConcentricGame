﻿using Meyham.EditorHelpers;
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
        
        [ReadOnly] public int PositionIndex;

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

        public void SetLeftButton(int button) => input.LeftButton = button;

        public void SetRightButton(int button) => input.RightButton = button;

        public void SetStartingPosition(int index, float angle)
        {
            PositionIndex = index;
            movement.SnapToStartingAngle(angle);
        }

        public void SetPlayerNumber(int number) => score.PlayerNumber = number;

        public void SetPlayerColor(Color playerColor)
        {
            PlayerColor = playerColor;
            spriteRenderer.color = playerColor;
        }

        public void ChangeOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
            Order = order;
            playerOrder.OrderPlayer(order);
            playerCollider.isTrigger = order == 0;
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