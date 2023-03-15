using System.Collections;
using Meyham.GameMode;
using Meyham.Player;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class ScoringStep : AGameStep
    {
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
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }

        public override void Activate()
        {
            var numberOfPlayers = playerManager.PlayerCount;
            
            if (playerScores != null && playerScores.Length == numberOfPlayers)
            {
                base.Activate();
                return;
            }

            var players = playerManager.GetPlayers();

            playerScores = new PlayerScore[numberOfPlayers];

            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerScores[i] = players[i].GetComponent<PlayerScore>();
            }
            
            base.Activate();
        }

        private void OnEnable()
        {
            scoreboard.SetScores(playerScores);
            StartCoroutine(DisplayScoreboard());
        }

        private IEnumerator DisplayScoreboard()
        {
            yield return scoreboard.OpenView();

            yield return new WaitForSeconds(endOfGameDelay);
            
            Deactivate();
        }
    }
}