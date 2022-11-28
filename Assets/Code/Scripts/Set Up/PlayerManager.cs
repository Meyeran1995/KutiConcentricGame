using System.Collections.Generic;
using System.Linq;
using Meyham.DataObjects;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.Set_Up
{
    public class PlayerManager : AGameLoopSystem
    {
        [Header("References")]
        [SerializeField] private GameObject playerTemplate;
        [SerializeField] private PlayerColors playerColors;
        [Header("Events")]
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;
        
        private static readonly Dictionary<int, PlayerController> Players = new();
        public static int PlayerCount;

        private PlayerController[] playersAsArray;
        private PlayerStartingPositionProvider startingPositionProvider;
        
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

        private void OnPlayerJoined(int inputIndex)
        {
            if (Players.TryGetValue(inputIndex, out var player))
            {
                var playerObject = player.gameObject;
                playerObject.SetActive(!playerObject.activeSelf);
                return;
            }
            
            CreateNewPlayer(inputIndex);
        }

        private void CreateNewPlayer(int inputIndex)
        {
            var newPlayer = Instantiate(playerTemplate).GetComponent<PlayerController>();
            newPlayer.name = $"Player{inputIndex}";
            newPlayer.transform.position = transform.position;
            newPlayer.enabled = false;
            newPlayer.PlayerColor = playerColors.GetColor(inputIndex);
            
            newPlayer.SetButton(inputIndex);
            newPlayer.SetPlayerNumber(inputIndex);
            newPlayer.SetStartingPosition(0f);

            Players.Add(inputIndex, newPlayer);
        }

        private void RemoveInactivePlayers()
        {
            for (int i = 0; i < 6; i++)
            {
                if(!Players.TryGetValue(i, out var player)) continue;
                
                var playerObject = player.gameObject;
                if(playerObject.activeSelf) continue;

                Players.Remove(i);
                Destroy(playerObject);
            }

            PlayerCount = Players.Count;
        }
        
        private void ShufflePlayers()
        {
            int n = PlayerCount;
            
            for (int i = 0; i < n - 1; i++)
            {
                int r = i + Random.Range(0, n - i);
                var t = playersAsArray[r];
                playersAsArray[r] = playersAsArray[i];
                playersAsArray[i] = t;
            }
        }

        private void StartPlayers()
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                var player = playersAsArray[i];
                player.SetStartingPosition(startingPositionProvider.GetStartingPosition(i));
                player.enabled = true;
            }
        }

        protected override void OnGameStart()
        {
            if (startingPositionProvider != null)
            {
                ShufflePlayers();
                return;
            }

            RemoveInactivePlayers();
            inputEventChannel -= OnPlayerJoined;
            startingPositionProvider = new PlayerStartingPositionProvider(PlayerCount);
            playersAsArray ??= Players.Values.ToArray();
            StartPlayers();
        }

        protected override void OnGameEnd()
        {
            foreach (var player in playersAsArray)
            {
                player.OnGameEnd();
            }
        }

        protected override void OnGameRestart()
        {
            startingPositionProvider.RotateStartingPositions();
            
            foreach (var player in playersAsArray)
            {
                player.OnGameRestart();
            }
            
            StartPlayers();
        }

        private void Awake()
        {
            inputEventChannel += OnPlayerJoined;
        }

#if UNITY_EDITOR

        [Header("Debug"), SerializeField] private FloatValue radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

#endif
    }
}