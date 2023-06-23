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
        
        public override void Setup()
        {
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
            loop.LinkScoreboardView(LinkView);
        }

        private void LinkView(ScoreboardView view)
        {
            scoreboard = view;
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
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