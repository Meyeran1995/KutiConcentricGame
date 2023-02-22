using Meyham.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.UI
{
    public class ScoreboardView : AGameView, IColoredText
    {
        [Header("Layout")]
        [SerializeField] private float screenWidth;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        [Header("Template")]
        [SerializeField] private float templateWidth;
        
        private ScoreBoardEntry[] scoreBoardEntries;

        public void SetScores(PlayerScore[] playerScores)
        {
            foreach (var playerScore in playerScores)
            {
                scoreBoardEntries[playerScore.PlayerNumber].SetScore(playerScore.GetScoreText());
            }
        }
        
        public void SetTextColor(int playerId, Color color)
        {
            scoreBoardEntries[playerId].SetEntryColor(color);
        }

        public void OnPlayerSelectionFinished()
        {
            PrepareEntries();
            CenterScoreboardEntries();
        }
        
        protected override void Awake()
        {
            base.Awake();
            scoreBoardEntries = transform.GetChild(0).GetComponentsInChildren<ScoreBoardEntry>();
            foreach (var entry in scoreBoardEntries)
            {
                entry.gameObject.SetActive(false);
            }
        }

        private void CenterScoreboardEntries()
        {
            float spacedWidth = templateWidth + layoutGroup.spacing;
            float scoreBoardWidth = (scoreBoardEntries.Length - 1) * spacedWidth + templateWidth;
            layoutGroup.padding.left = (int)(screenWidth - scoreBoardWidth) / 2;
        }

        private void PrepareEntries()
        {
            var root = transform.GetChild(0);
            
            for (int i = 0; i < scoreBoardEntries.Length; i++)
            {
                var entry = scoreBoardEntries[i];
                entry.SetPlayerName(i);
                entry.transform.SetParent(root);
                entry.gameObject.SetActive(true);
            }
        }
    }
}