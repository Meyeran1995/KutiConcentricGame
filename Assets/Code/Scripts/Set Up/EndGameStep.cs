using System.Collections;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class EndGameStep : AGameStep
    {
        [SerializeField] private GenericEventChannelSO<int> inputEventChannel;

        private PlayerManager playerManager;

        private ScoreboardView scoreboard;

        private InGameView inGameView;
        
        public override void Setup()
        {
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
            loop.LinkScoreboardView(LinkScoreboard);
            loop.LinkInGameView(LinkInGameView);
        }

        private void LinkScoreboard(ScoreboardView view)
        {
            scoreboard = view;
        }
        
        private void LinkInGameView(InGameView view)
        {
            inGameView = view;
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }

        public override void Deactivate()
        {
            scoreboard.Clean();
            inGameView.Clean();
            base.Deactivate();
        }

        private void OnEnable()
        {
            Alerts.SendAlert("Drücken für Neustart");
            inputEventChannel += OnRestartPressed;
        }

        private void OnRestartPressed(int input)
        {
            inputEventChannel -= OnRestartPressed;
            Alerts.ClearAlert();
            playerManager.ShufflePlayers();
            StartCoroutine(EndOfGame());
        }

        private IEnumerator EndOfGame()
        {
            yield return scoreboard.CloseView();
            
            Deactivate();
        }
    }
}