using System;
using Meyham.Events;
using Meyham.GameMode;
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
        [SerializeField] private float templateWidth;
        
        private ScoreBoardEntry[] scoreBoardEntries;

        public override void OpenView(int animatorId)
        {
            ShowScores();
            base.OpenView(animatorId);
        }

        // public override void CloseView(int animatorId)
        // {
        //     base.CloseView(animatorId);
        // }
        
        public override void SetTextColor(int playerId, Color color)
        {
            var entry = scoreBoardEntries[playerId];
            entry.gameObject.SetActive(true);
            entry.SetEntryColor(color);
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

        private void Start()
        {
            gameStart += OnPlayerSelectionFinished;
        }

        private void OnPlayerSelectionFinished()
        {
            gameStart -= OnPlayerSelectionFinished;

            PrepareEntries();
            CenterScoreboardEntries();
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
                scoreBoardEntries[i] = entry;
            }
        }
        
        private void ShowScores()
        {
            foreach (var playerScore in ScoreKeeper.GetScores())
            {
                scoreBoardEntries[playerScore.PlayerNumber].SetScore(playerScore.GetScoreText());
            }
        }
    }
}