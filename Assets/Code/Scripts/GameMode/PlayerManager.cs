using System.Collections.Generic;
using Meyham.Player;
using UnityEngine;

namespace Meyham.GameMode
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject playerTemplate;
        
        private Dictionary<int, PlayerController> players;

        public void OnPlayerJoined(int inputIndex)
        {
            var newPlayer = Instantiate(playerTemplate).GetComponent<PlayerController>();
            newPlayer.enabled = false;
            newPlayer.SetLeftButton(inputIndex);
            
            players.Add(inputIndex, newPlayer);
        }

        public void OnGameStart(int numberOfPlayers)
        {
            float positionGain = 360f / numberOfPlayers;
            float currentAngle = 0f;

            foreach (var player in players.Values)
            {
                player.SetStartingPosition(currentAngle);
                player.enabled = true;
                currentAngle += positionGain;
            }
        }

        private void Awake()
        {
            players = new Dictionary<int, PlayerController>();
        }
    }
}