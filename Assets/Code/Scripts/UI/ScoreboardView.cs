using Meyham.Player;
using Meyham.Set_Up;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.UI
{
    public class ScoreboardView : AGameView, IColoredText, IPlayerNumberDependable
    {
        [Header("Layout")]
        [SerializeField] private float screenWidth;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        
        [Header("Template")]
        [SerializeField] private float templateWidth;
        [SerializeField] private ScoreBoardEntry[] scoreBoardEntries;

        public void SetTextColor(int playerId, Color color)
        {
            scoreBoardEntries[playerId].SetEntryColor(color);
        }

        public void OnPlayerJoined(int playerNumber)
        {
            scoreBoardEntries[playerNumber].gameObject.SetActive(true);
        }

        public void OnPlayerLeft(int playerNumber)
        {
            scoreBoardEntries[playerNumber].gameObject.SetActive(false);
        }
        
        public void SetScores(PlayerScore[] playerScores)
        {
            foreach (var playerScore in playerScores)
            {
                scoreBoardEntries[(int)playerScore.Designation].SetScore(playerScore.GetScoreText());
            }
        }

        protected override void Awake()
        {
            foreach (var entry in scoreBoardEntries)
            {
                entry.gameObject.SetActive(false);
            }
            base.Awake();
        }
    }
}