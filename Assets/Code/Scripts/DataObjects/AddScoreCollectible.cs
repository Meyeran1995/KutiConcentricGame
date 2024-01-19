using Meyham.Events;
using Meyham.Player;
using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/Collectibles/AddScore")]
    public class AddScoreCollectible : ACollectibleData
    {
        [Header("References")]
        [SerializeField] private PlayerScoreEventChannelSO collectionEvent;
        
        [Header("Value")]
        [SerializeField] private float score;

        public override void Collect(GameObject player)
        {
            var playerScore = player.GetComponentInParent<PlayerScore>();
            playerScore.AcquireScore(score);
            
            collectionEvent.RaiseEvent(playerScore);
        }
    }
}
