using Meyham.Events;
using Meyham.Player;
using UnityEngine;

namespace Meyham.Items
{
    public class AddScoreCollectible : ACollectible
    {
        [Header("References")]
        [SerializeField] private PlayerScoreEventChannelSO collectionEvent;
        
        [field: Header("Values"), SerializeField] public float Score;
        
        protected override void OnCollect(GameObject player)
        {
            var playerScore = player.GetComponentInParent<PlayerScore>();
            playerScore.AcquireScore(Score);
            
            collectionEvent.RaiseEvent(playerScore);
        }
    }
}
