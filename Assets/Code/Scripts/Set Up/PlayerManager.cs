using System.Collections.Generic;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class PlayerManager : AGameLoopSystem
    {
        [Header("References")]
        [SerializeField] private GameObject playerTemplate;
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;

        private static readonly Dictionary<int, PlayerController> players = new();

        public static int NumberOfActivePlayers => players.Count;

        protected override void Start()
        {
            base.Start();
            inputEventChannel += OnPlayerJoined;
        }

        private void OnPlayerJoined(int inputIndex)
        {
            if(players.ContainsKey(inputIndex)) return;
            
            var newPlayer = Instantiate(playerTemplate).GetComponent<PlayerController>();
            newPlayer.enabled = false;
            newPlayer.SetLeftButton(inputIndex);
            
            players.Add(inputIndex, newPlayer);
        }

        protected override void OnGameStart()
        {
            float positionGain = 360f / players.Count;
            float currentAngle = 0f;

            foreach (var player in players.Values)
            {
                player.SetStartingPosition(currentAngle);
                player.enabled = true;
                currentAngle += positionGain;
            }

            inputEventChannel -= OnPlayerJoined;
        }

        protected override void OnGameEnd()
        {
            foreach (var player in players.Values)
            {
                player.OnGameEnd();
            }
        }

        protected override void OnGameRestart()
        {
            foreach (var player in players.Values)
            {
                player.OnGameRestart();
            }
        }
    }
}