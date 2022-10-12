using System.Collections.Generic;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.Player;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.Set_Up
{
    public class PlayerManager : AGameLoopSystem
    {
        [Header("References")]
        [SerializeField] private GameObject playerTemplate;
        [SerializeField] private PlayerColors playerColors;
        [SerializeField] private PlayerPositionTracker positionTracker;
        
        [Header("Events")]
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;
        
        private static readonly Dictionary<int, PlayerController> Players = new();

        public static int NumberOfActivePlayers => Players.Count;

        public static PlayerScore[] GetPlayers()
        {
            var playerScores = new PlayerScore[Players.Count];
            int i = 0;

            foreach (var controller in Players.Values)
            {
                playerScores[i++] = controller.Score;
            }

            return playerScores;
        }
        
        protected override void Start()
        {
            base.Start();
            inputEventChannel += OnPlayerJoined;
        }

        private void OnPlayerJoined(int inputIndex)
        {
            if(Players.ContainsKey(inputIndex)) return;
            
            var newPlayer = Instantiate(playerTemplate, transform.position, quaternion.identity)
                .GetComponent<PlayerController>();
            newPlayer.enabled = false;
            newPlayer.SetLeftButton(inputIndex);
            newPlayer.SetPlayerNumber(inputIndex);
            newPlayer.SetStartingPosition(0, 0f);
            newPlayer.SetPlayerColor(playerColors.GetColor(inputIndex));

            Players.Add(inputIndex, newPlayer);
        }

        protected override void OnGameStart()
        {
            positionTracker.InitStartingPositions(Players.Count);

            int i = 0;
            foreach (var player in Players.Values)
            {
                positionTracker.GetStartingPosition(player, i);
                i++;
                player.enabled = true;
            }

            inputEventChannel -= OnPlayerJoined;
        }

        protected override void OnGameEnd()
        {
            foreach (var player in Players.Values)
            {
                player.OnGameEnd();
            }
        }

        protected override void OnGameRestart()
        {
            positionTracker.RotateStartingPositions();
            
            float[] startingPositions = positionTracker.StartingPositions;
            int i = 0;
            foreach (var player in Players.Values)
            {
                player.SetStartingPosition(i, startingPositions[i++]);
                player.OnGameRestart();
            }
        }

#if UNITY_EDITOR

        private RadialPlayerMovement movement;
        
        private void OnDrawGizmos()
        {
            if (movement == null)
            {
                if(!playerTemplate.TryGetComponent(out movement)) return;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, movement.Radius);
        }

#endif
    }
}