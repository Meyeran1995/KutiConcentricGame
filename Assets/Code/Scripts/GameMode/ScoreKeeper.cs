using System.Collections.Generic;
using Meyham.Events;
using Meyham.Player;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.GameMode
{
    public class ScoreKeeper : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO gameStartEvent;
        
        private static readonly List<PlayerScore> Scores = new();

        public static List<PlayerScore> GetScores() => Scores;
        
        private void Start()
        {
            gameStartEvent += OnGameStart;
        }

        private void OnGameStart()
        {
            gameStartEvent -= OnGameStart;

            foreach (var playerScore in PlayerManager.GetPlayers())
            {
                Scores.Add(playerScore);
            }
        }
    }
}