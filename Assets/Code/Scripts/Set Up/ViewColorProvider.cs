using Meyham.Events;
using Meyham.UI;

namespace Meyham.Set_Up
{
    public class ViewColorProvider
    {
        private AGameView[] views;
        private PlayerColors playerColors;

        public ViewColorProvider(AGameView[] views, PlayerColors playerColors)
        {
            this.views = views;
            this.playerColors = playerColors;
        }

        public void SubscribeToEvent(GenericEventChannelSO<int> playerJoined)
        {
            playerJoined += OnPlayerJoined;
        }
        
        public void UnSubscribeFromEvent(GenericEventChannelSO<int> playerJoined)
        {
            playerJoined -= OnPlayerJoined;
        }

        private void OnPlayerJoined(int playerNumber)
        {
            foreach (var view in views)
            {
                view.SetTextColor(playerNumber, playerColors.GetColor(playerNumber));
            }
        }
    }
}