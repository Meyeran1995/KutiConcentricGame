using System.Collections.Generic;
using Meyham.DataObjects;
using Meyham.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.Set_Up
{
    public class PlayerManager : MonoBehaviour, IPlayerNumberDependable
    {
        [Header("References")]
        [SerializeField] private GameObject playerTemplate;
        [SerializeField] private PlayerColors playerColors;
        
        private static readonly Dictionary<int, PlayerController> Players = new();

        private PlayerController[] playersAsArray;

        public PlayerController[] GetPlayers()
        {
            if (playersAsArray != null) return playersAsArray;
            
            playersAsArray = new PlayerController[Players.Count];

            int i = 0;

            foreach (var player in Players.Values)
            {
                playersAsArray[i] = player;
                i++;
            }

            return playersAsArray;
        }
        
        public void OnPlayerJoined(int inputIndex)
        {
            if (Players.TryGetValue(inputIndex, out var player))
            {
                player.gameObject.SetActive(true);
                return;
            }
            
            CreateNewPlayer(inputIndex);
        }

        public void OnPlayerLeft(int inputIndex)
        {
            Players[inputIndex].gameObject.SetActive(false);
        }

        public void EnablePlayers()
        {
            foreach (var player in playersAsArray)
            {
                player.enabled = true;
            }
        }

        public void DisablePlayers()
        {
            foreach (var player in playersAsArray)
            {
                player.enabled = false;
            }
        }

        public void RemoveInactivePlayers()
        {
            for (int i = 0; i < 6; i++)
            {
                if(!Players.TryGetValue(i, out var player)) continue;
                
                var playerObject = player.gameObject;
                
                if(playerObject.activeSelf) continue;

                Players.Remove(i);
                Destroy(playerObject);
            }
        }
        
        public void ShufflePlayers()
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
        
        private void CreateNewPlayer(int inputIndex)
        {
            var newPlayer = Instantiate(playerTemplate).GetComponent<PlayerController>();
            newPlayer.name = $"Player{inputIndex}";
            newPlayer.transform.position = transform.position;
            newPlayer.enabled = false;
            newPlayer.PlayerColor = playerColors[inputIndex];
            
            newPlayer.SetButton(inputIndex);
            newPlayer.SetPlayerNumber(inputIndex);
            newPlayer.SetStartingPosition(0f);

            Players.Add(inputIndex, newPlayer);
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