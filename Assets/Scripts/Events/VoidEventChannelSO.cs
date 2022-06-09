using UnityEngine;
using UnityEngine.Events;

namespace Meyham.Events
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        private UnityAction onEventRaised;
        
#if UNITY_EDITOR
        public bool HasListeners => onEventRaised != null;
#endif
        
        public void RaiseEvent()
        {
            if (onEventRaised != null)
            {
                onEventRaised.Invoke();
            }
            else
            {
                Debug.LogWarning($"Void Event Channel {name} has no listeners");
            }
        }

        public static VoidEventChannelSO operator +(VoidEventChannelSO channel, UnityAction listener)
        {
            channel.onEventRaised += listener;
            return channel;
        }

        public static VoidEventChannelSO operator -(VoidEventChannelSO channel, UnityAction listener)
        {
            channel.onEventRaised -= listener;        
            return channel;
        }
    }
}
