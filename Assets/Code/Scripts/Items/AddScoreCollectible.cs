using Meyham.Events;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    public class AddScoreCollectible : ACollectible
    {
        [Header("References")]
        [SerializeField] private PlayerScoreEventChannelSO collectionEvent;
        
        [Header("Values")]
        [SerializeField] private float score;
        
        protected override void OnCollect(GameObject player)
        {
            var playerScore = player.GetComponentInParent<PlayerScore>();
            playerScore.AcquireScore(score);
            
            collectionEvent.RaiseEvent(playerScore);
        }
    }
}
