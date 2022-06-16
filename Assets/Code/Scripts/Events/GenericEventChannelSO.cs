using UnityEngine;
using UnityEngine.Events;

namespace Meyham.Events
{
    public class GenericEventChannelSO<T> : ScriptableObject
    {
        
#if UNITY_EDITOR
        public bool HasListeners => onEventRaised != null;
#endif
        
        protected UnityAction<T> onEventRaised;

        public void RaiseEvent(T state)
        {
            if (onEventRaised != null)
            {
                onEventRaised.Invoke(state);
            }
            else
            {
                Debug.LogWarning($"{typeof(T)} Event Channel {name} has no listeners");
            }
        }

        public static GenericEventChannelSO<T> operator +(GenericEventChannelSO<T> channel, UnityAction<T> listener)
        {
            channel.onEventRaised += listener;
            return channel;
        }

        public static GenericEventChannelSO<T> operator -(GenericEventChannelSO<T> channel, UnityAction<T> listener)
        {
            channel.onEventRaised -= listener;
            return channel;
        }
    }
}
