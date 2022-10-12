﻿using System;
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

        public Color PlayerColor { get; private set; }

        public PlayerScore Score => score;
        public RadialPlayerMovement Movement => movement;
        
        private static readonly int ColorId = Shader.PropertyToID("_Color");

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
            movement.PositionIndex = index;
            movement.SnapToStartingAngle(angle);
        }

        public void SetPlayerNumber(int number) => score.PlayerNumber = number;

        public void SetPlayerColor(Color playerColor)
        {
            PlayerColor = playerColor;
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(ColorId, PlayerColor);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        public void ChangeOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
            movement.Order = order;
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