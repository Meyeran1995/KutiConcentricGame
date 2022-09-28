using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    public class PowerUp : ACollectible
    {
        public APowerUpEffect Effect;
        
        protected override void OnCollect(GameObject player)
        {
            var playerScore = player.GetComponent<PlayerPowerUpReceiver>();
            playerScore.Receive(Effect);
        }
    }
}
