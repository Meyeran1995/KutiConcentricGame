using System.Collections.Generic;
using Meyham.Events;
using Meyham.Player;
using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class InGameView : AGameView
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI[] scoreTexts;
        [Header("Events")]
        [SerializeField] private GenericEventChannelSO<PlayerScore> scoreEvent;
        [SerializeField] private GenericEventChannelSO<int> playerJoinedEvent;
        [SerializeField] private VoidEventChannelSO gameStartEvent;

        private readonly List<int> activePlayers = new();
        
        // public override void OpenView(int animatorId)
        // {
        //     base.OpenView(animatorId);
        // }
        
        public override void CloseView(int animatorId)
        {
            ResetScores();
            base.CloseView(animatorId);
        }

        public override void SetTextColor(int playerId, Color color)
        {
            scoreTexts[playerId].color = color;
        }

        protected override void Awake()
        {
            foreach (var text in scoreTexts)
            {
                text.gameObject.SetActive(false);
            }
        }
        
        private void Start()
        {
            scoreEvent += OnScoreAcquired;
            gameStartEvent += OnGameStart;
            playerJoinedEvent += OnPlayerJoined;
        }

        private void OnGameStart()
        {
            playerJoinedEvent -= OnPlayerJoined;
        }

        private void OnPlayerJoined(int playerNumber)
        {
            var scoreTextGameObject = scoreTexts[playerNumber].gameObject;
            
            if (activePlayers.Contains(playerNumber))
            {
                scoreTextGameObject.SetActive(!scoreTextGameObject.activeSelf);
                activePlayers.Remove(playerNumber);
                return;
            }
            
            activePlayers.Add(playerNumber);
            scoreTextGameObject.SetActive(true);
        }

        private void OnScoreAcquired(PlayerScore scoringPlayer)
        {
            scoreTexts[scoringPlayer.PlayerNumber].text = scoringPlayer.GetScoreText();
        }

        private void ResetScores()
        {
            foreach (var player in activePlayers)
            {
                scoreTexts[player].text = "0";
            }
        }
    }
}