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
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;

        private float[] startingPositions;
        
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
            newPlayer.SetStartingPosition(0f);
            newPlayer.SetPlayerColor(playerColors.GetColor(inputIndex));

            Players.Add(inputIndex, newPlayer);
        }

        protected override void OnGameStart()
        {
            GetStartingPositions();

            int i = 0;
            foreach (var player in Players.Values)
            {
                player.SetStartingPosition(startingPositions[i++]);
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
            RotateStartPositions();
            
            int i = 0;
            foreach (var player in Players.Values)
            {
                player.SetStartingPosition(startingPositions[i++]);
                player.OnGameRestart();
            }
        }

        private void GetStartingPositions()
        {
            int playerCount = Players.Count;
            float positionGain = 360f / playerCount;
            float currentAngle = 0f;
            startingPositions = new float[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                startingPositions[i] = currentAngle;
                currentAngle += positionGain;
            }
        }

        private void RotateStartPositions()
        {
            float rotation = Random.Range(10f, 350f);

            for (int i = 0; i < startingPositions.Length; i++)
            {
                float position = startingPositions[i];
                position += rotation;
                position %= 360f;
                startingPositions[i] = position;
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