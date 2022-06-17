using Meyham.DataObjects;
using Meyham.Events;
using UnityEngine;

namespace Meyham.Items
{
    public class AddTimeCollectible : ACollectible
    {
        [Header("References")]
        [SerializeField] private FloatValue endTimer;
        [SerializeField] private VoidEventChannelSO collectionEvent;
        
        [Header("Values")]
        [SerializeField] private float timeToAdd;
        
        protected override void OnCollect()
        {
            Debug.Log($"You gained {timeToAdd} time");
            endTimer.RuntimeValue += timeToAdd;
            collectionEvent.RaiseEvent();
        }
    }
}
