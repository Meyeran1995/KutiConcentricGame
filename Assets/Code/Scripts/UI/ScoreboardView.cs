using System;
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
            base.Awake();
            foreach (var entry in scoreBoardEntries)
            {
                entry.gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if(scoreBoardEntries == null) return;

            for (int i = 0; i < scoreBoardEntries.Length; i++)
            {
                scoreBoardEntries[i].SetPlayerName(GetPlayerName(i));
            }
        }

        private string GetPlayerName(int index)
        {
            var designation = (PlayerDesignation)index;
            string playerName = $"Spieler {index}";
            
            switch (designation)
            {
                case PlayerDesignation.Orange:
                    playerName = "Orange";
                    break;
                case PlayerDesignation.Green:
                    playerName = "Grün";
                    break;
                case PlayerDesignation.Purple:
                    playerName = "Lila";
                    break;
                case PlayerDesignation.Yellow:
                    playerName = "Gelb";
                    break;
                case PlayerDesignation.Red:
                    playerName = "Rot";
                    break;
                case PlayerDesignation.Cyan:
                    playerName = "Türkis";
                    break;
                default:
                    Debug.LogWarning($"No name defined for Playerdesignation {designation}");
                    break;
            }

            return playerName;
        }

#endif
        
    }
}