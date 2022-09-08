using Meyham.Events;
using TMPro;
using UnityEngine;

namespace Meyham.UI
{
    public class MainMenuView : AGameView
    {
        [Header("Players")]
        [SerializeField] private GenericEventChannelSO<int> onPlayerJoined;
        [SerializeField] private TextMeshProUGUI[] playerJoinTexts;
        
        private int numberOfPlayers;
        
        private void Start()
        {
            onPlayerJoined += OnPlayerJoin;
        }

        private void OnPlayerJoin(int playerIndex)
        {
            var playerText = playerJoinTexts[playerIndex];
            
            if(playerText.text[0] == 'S') return;

            playerText.text = $"Spieler {++numberOfPlayers}";
        }
        
        public override void OpenView(int animatorId)
        {
        }

        public override void CloseView(int animatorId)
        {
            onPlayerJoined -= OnPlayerJoin;
            Alerts.ClearAlert();
        }
    }
}