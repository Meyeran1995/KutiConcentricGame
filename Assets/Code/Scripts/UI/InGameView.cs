using System.Collections.Generic;
using Meyham.Events;
using Meyham.Player;
using Meyham.Set_Up;
using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class InGameView : AGameView, IColoredText, IPlayerNumberDependable
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI[] scoreTexts;
        [Header("Events")]
        [SerializeField] private GenericEventChannelSO<PlayerScore> scoreEvent;

        private readonly List<int> activePlayers = new(6);

        public override void OpenView()
        {
            scoreEvent += OnScoreAcquired;

            base.OpenView();
        }

        public override void CloseView()
        {
            ResetScores();
            scoreEvent -= OnScoreAcquired;

            base.CloseView();
        }

        public void SetTextColor(int playerId, Color color)
        {
            scoreTexts[playerId].color = color;
        }
        
        public void OnPlayerJoined(int playerNumber)
        {
            activePlayers.Add(playerNumber);
            scoreTexts[playerNumber].gameObject.SetActive(true);
        }

        public void OnPlayerLeft(int playerNumber)
        {
            scoreTexts[playerNumber].gameObject.SetActive(false);
            activePlayers.Remove(playerNumber);
        }

        protected override void Awake()
        {
            foreach (var text in scoreTexts)
            {
                text.gameObject.SetActive(false);
            }
        }

        private void OnScoreAcquired(PlayerScore scoringPlayer)
        {
            scoreTexts[(int)scoringPlayer.Designation].text = scoringPlayer.GetScoreText();
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