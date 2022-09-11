using Meyham.Events;
using Meyham.Player;
using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] scoreTexts;
        [SerializeField] private GenericEventChannelSO<PlayerScore> scoreEvent;

        
        
        //TODO: Need to hide ui based on inputs
        
        public void ResetScores()
        {
            foreach (var scoreText in scoreTexts)
            {
                scoreText.text = "0";
            }
        }
        
        private void Start()
        {
            scoreEvent += OnScoreAcquired;
        }

        private void OnScoreAcquired(PlayerScore scoringPlayer)
        {
            scoreTexts[scoringPlayer.PlayerNumber].text = scoringPlayer.GetScore();
        }
    }
}