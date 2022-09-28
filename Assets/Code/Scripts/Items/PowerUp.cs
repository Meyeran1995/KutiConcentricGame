using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    public class PowerUp : ACollectible
    {
        [SerializeField] private APowerUpEffect effect;
        
        protected override void OnCollect(GameObject player)
        {
            var playerScore = player.GetComponent<PlayerPowerUpReceiver>();
            playerScore.Receive(effect);
        }
    }
}
