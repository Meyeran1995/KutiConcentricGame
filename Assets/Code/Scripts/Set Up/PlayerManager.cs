using System.Collections.Generic;
using System.Linq;
using Meyham.DataObjects;
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

        private PlayerController[] playersAsArray;
        private int indexGain;
        
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
            newPlayer.SetButton(inputIndex);
            newPlayer.SetPlayerNumber(inputIndex);
            newPlayer.SetStartingPosition(0, 0f);
            newPlayer.SetPlayerColor(playerColors.GetColor(inputIndex));

            Players.Add(inputIndex, newPlayer);
        }

        protected override void OnGameStart()
        {
            int i = 0;
            int playerCount = Players.Count;
            PlayerPositionTracker.InitializeLists(playerCount);
            
            if (indexGain == 0)
            {
                inputEventChannel -= OnPlayerJoined;
                indexGain = Mathf.FloorToInt((float)PlayerPositionTracker.MaxPosition / playerCount );
            }
            
            if (playerCount > 1)
            {
                foreach (var player in Players.Values)
                {
                    player.SetStartingPosition(i,positionTracker.GetStartingPosition(i));
                    i += indexGain;
                    player.enabled = true;
                }
                return;                
            }

            playersAsArray ??= Players.Values.ToArray();
            
            ShufflePlayers();

            foreach (var player in playersAsArray)
            {
                player.SetStartingPosition(i,positionTracker.GetStartingPosition(i));
                i += indexGain;
                player.enabled = true;
            }
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

        private void ShufflePlayers()
        {
            int n = playersAsArray.Length;
            
            for (int i = 0; i < n - 1; i++)
            {
                int r = i + Random.Range(0, n - i);
                var t = playersAsArray[r];
                playersAsArray[r] = playersAsArray[i];
                playersAsArray[i] = t;
            }
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