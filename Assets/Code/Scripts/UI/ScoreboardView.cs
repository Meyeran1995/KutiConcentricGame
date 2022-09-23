using System.Collections;
using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.Set_Up;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.UI
{
    public class ScoreboardView : AGameView
    {
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO gameStart;
        [Header("Layout")]
        [SerializeField] private float screenWidth;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        [Header("Template")]
        [SerializeField] private GameObject boardTemplate;
        [SerializeField] private float templateWidth;
        [Header("Debug")]
        [ReadOnly, SerializeField] private ScoreBoardEntry[] scoreBoardEntries;
        
        public override void OpenView(int animatorId)
        {
            ShowScores();
            base.OpenView(animatorId);
        }

        // public override void CloseView(int animatorId)
        // {
        //     base.CloseView(animatorId);
        // }
        
        private void Start()
        {
            gameStart += OnPlayerSelectionFinished;
        }

        private void OnPlayerSelectionFinished()
        {
            gameStart -= OnPlayerSelectionFinished;

            int numberOfPlayers = PlayerManager.NumberOfActivePlayers;
            scoreBoardEntries = new ScoreBoardEntry[numberOfPlayers];

            StartCoroutine(SpawnEntries(numberOfPlayers));
            CenterScoreboardEntries(numberOfPlayers);
        }

        private void CenterScoreboardEntries(int numberOfPlayers)
        {
            float spacedWidth = templateWidth + layoutGroup.spacing;
            float scoreBoardWidth = (numberOfPlayers - 1) * spacedWidth + templateWidth;
            layoutGroup.padding.left = (int)(screenWidth - scoreBoardWidth) / 2;
        }

        private IEnumerator SpawnEntries(int numberOfPlayers)
        {
            var root = transform.GetChild(0);
            
            for (int i = 0; i < numberOfPlayers; i++)
            {
                var entry = Instantiate(boardTemplate, transform).GetComponent<ScoreBoardEntry>();
                entry.SetPlayerName(i);
                entry.transform.SetParent(root);
                scoreBoardEntries[i] = entry;
                yield return null;
            }
        }
        
        private void ShowScores()
        {
            var playerScores = ScoreKeeper.GetScores();

            for (int i = 0; i < playerScores.Length; i++)
            {
                scoreBoardEntries[i].SetScore(playerScores[i].ToString());
            }
        }
    }
}