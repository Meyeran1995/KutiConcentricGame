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
        
        public override void SeTup()
        {
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
        }

        public override void Activate()
        {
            Alerts.SendAlert("Drücken für Neustart");
            base.Activate();
        }
        
        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }

        private void OnEnable()
        {
            inputEventChannel += OnRestartPressed;
        }

        private void OnDisable()
        {
            inputEventChannel -= OnRestartPressed;
        }

        private void OnRestartPressed(int input)
        {
            Alerts.ClearAlert();
            playerManager.ShufflePlayers();
            Deactivate();
        }
    }
}