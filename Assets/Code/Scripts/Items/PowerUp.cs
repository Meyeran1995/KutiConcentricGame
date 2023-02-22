using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    public class PowerUp : ACollectible
    {
        public APowerUpEffect Effect;

        public override void Collect(GameObject player)
        {
            var playerPowerUpReceiver = player.GetComponent<PlayerPowerUpReceiver>();
            playerPowerUpReceiver.Receive(Effect);
        }
    }
}
