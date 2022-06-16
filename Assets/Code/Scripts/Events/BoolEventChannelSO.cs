using UnityEngine;
using UnityEngine.Events;

namespace Meyham.Events
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Events/Bool Event Channel")]
    public class BoolEventChannelSO : GenericEventChannelSO<bool>
    {
#if UNITY_EDITOR
    
        public void InvokeTrue() => RaiseEvent(true);
        public void InvokeFalse() => RaiseEvent(false);

#endif
        
        public static BoolEventChannelSO operator +(BoolEventChannelSO channel, UnityAction<bool> listener)
        {
            channel.onEventRaised += listener;
            return channel;
        }

        public static BoolEventChannelSO operator -(BoolEventChannelSO channel, UnityAction<bool> listener)
        {
            channel.onEventRaised -= listener;
            return channel;
        }
    }
}
