using Meyham.DataObjects;
using Meyham.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.Set_Up
{
    public class PlayerManager : MonoBehaviour, IPlayerNumberDependable
    {
        [Header("References")]
        [SerializeField] private PlayerColors playerColors;
        [SerializeField] private PlayerController[] players;

        public int PlayerCount { get; private set; }

        public PlayerController[] GetPlayers()
        {
            var currentPlayers = new PlayerController[PlayerCount];

            int i = 0;
            
            foreach (var player in players)
            {
                if(!player.IsActive) continue;

                currentPlayers[i] = player;
                i++;
            }
            
            return currentPlayers;
        }
        
        public void OnPlayerJoined(int inputIndex)
        {
            players[inputIndex].Activate();
        }

        public void OnPlayerLeft(int inputIndex)
        {
            players[inputIndex].Deactivate();
        }

        public void DisablePlayer(int designation)
        {
            foreach (var player in players)
            {
                if ((int)player.Designation != designation)
                {
                    continue;
                }

                player.enabled = false;
            }
        }

        public void EnablePlayers()
        {
            foreach (var player in players)
            {
                if(!player.IsActive) continue;

                player.enabled = true;
            }
        }

        public void DisablePlayers()
        {
            foreach (var player in players)
            {
                if(!player.IsActive) continue;

                player.enabled = false;
            }
        }
        
        public void ShowPlayers()
        {
            foreach (var player in players)
            {
                if(!player.IsActive) continue;

                player.ShowPlayer();
            }
        }
        
        public void HidePlayers()
        {
            foreach (var player in players)
            {
                if(!player.IsActive) continue;

                player.HidePlayer();
            }
        }

        public void UpdatePlayerCount()
        {
            PlayerCount = 0;
            foreach (var player in players)
            {
                if(!player.IsActive)
                {
                    player.gameObject.SetActive(false);
                    continue;
                }

                PlayerCount++;
            }
        }

        public void ShufflePlayers()
        {
            int n = players.Length;
            
            for (int i = 0; i < n - 1; i++)
            {
                int r = i + Random.Range(0, n - i);
                var t = players[r];
                players[r] = players[i];
                players[i] = t;
            }
        }

        private void Start()
        {
            foreach (var player in players)
            {
                player.SetPlayerColor(playerColors[(int)player.Designation]);
                player.HidePlayer();
            }
        }

#if UNITY_EDITOR

        [Header("Debug"), SerializeField] private FloatParameter radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private void OnValidate()
        {
            if (players == null) return;

            float originZ = transform.position.z;
            
            foreach (var player in players)
            {
                if (Mathf.Abs(player.transform.position.z - originZ) <= 0.0001f) continue;
                Debug.LogError($"Player {player.name} is not aligned to origin z position");
            }
        }

#endif
    }
}