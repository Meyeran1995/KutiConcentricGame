﻿using Meyham.Events;
using UnityEngine;

namespace Meyham.UI
{
    public class MainMenuView : AGameView
    {
        [Header("Players")]
        [SerializeField] private GenericEventChannelSO<int> onPlayerJoined;
        [SerializeField] private Toggle[] playerToggles;
        
        private void Start()
        {
            onPlayerJoined += OnPlayerJoin;
        }

        private void OnPlayerJoin(int playerIndex)
        {
            var toggle = playerToggles[playerIndex];
            toggle.ToggleImage();
        }
        
        // public override void OpenView(int animatorId)
        // {
        //     base.OpenView(animatorId);
        // }

        public override void CloseView(int animatorId)
        {
            base.CloseView(animatorId);
            onPlayerJoined -= OnPlayerJoin;
            Alerts.ClearAlert();
        }

        public override void SetTextColor(int playerId, Color color)
        {
        }
    }
}