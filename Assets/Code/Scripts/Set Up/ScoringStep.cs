using Meyham.Events;
using Meyham.GameMode;
using Meyham.Player;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class ScoringStep : AGameStep
    {
        [SerializeField] private VoidEventChannelSO lastItemVanishedEvent;
        [SerializeField] private float endOfGameDelay;

        private ScoreboardView scoreboard;

        private PlayerManager playerManager;
        
        private PlayerScore[] playerScores;

        public override void SeTup()
        {
            scoreboard = FindAnyObjectByType<ScoreboardView>(FindObjectsInactive.Include);
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
            lastItemVanishedEvent += OnLastItemVanished;
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }

        private void OnEnable()
        {
            var players = playerManager.GetPlayers();
            var numberOfPlayers = players.Length;
            
            if (playerScores == null || playerScores.Length == numberOfPlayers)
            {
                return;
            }

            playerScores = new PlayerScore[numberOfPlayers];

            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerScores[i] = players[i].GetComponent<PlayerScore>();
            }
        }

        private void OnLastItemVanished()
        {
            scoreboard.SetScores(playerScores);
            scoreboard.OpenView();
        }
    }
}