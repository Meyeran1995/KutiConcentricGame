using Meyham.Events;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    public class AddScoreCollectible : ACollectible
    {
        [Header("References")]
        [SerializeField] private PlayerScoreEventChannelSO collectionEvent;
        
        private float score;

        public void SetScore(float newScore)
        {
            score = newScore;
        }

        public override void Collect(GameObject player)
        {
            var playerScore = player.GetComponentInParent<PlayerScore>();
            playerScore.AcquireScore(score);
            
            collectionEvent.RaiseEvent(playerScore);
        }
    }
}
