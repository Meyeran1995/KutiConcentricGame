using System.Collections.Generic;
using UnityEngine;

namespace Meyham.Events
{
    /// <summary>
    /// Globally raises events based on input received
    /// </summary>
    public class InputEventDistributor : MonoBehaviour
    {
        [Header("P1 Events")] 
        [SerializeField] private BoolEventChannelSO p1LeftButton;
        [SerializeField] private BoolEventChannelSO p1MiddleButton, p1RightButton;

        [Header("P2 Events")] 
        [SerializeField] private BoolEventChannelSO p2LeftButton;
        [SerializeField] private BoolEventChannelSO p2MiddleButton, p2RightButton;

#if UNITY_ANDROID
    
    private Dictionary<EKutiButton, BoolEventChannelSO> bindings;

    private void Awake()
    {
        bindings = new Dictionary<EKutiButton, BoolEventChannelSO>
        {
            { EKutiButton.P1_LEFT, p1LeftButton },
            { EKutiButton.P1_MID, p1MiddleButton },
            { EKutiButton.P1_RIGHT, p1RightButton },
            { EKutiButton.P2_LEFT, p2LeftButton },
            { EKutiButton.P2_MID, p2MiddleButton },
            { EKutiButton.P2_RIGHT, p2RightButton }
        };
    }


    private void Update()
    {
        if(!Input.anyKeyDown) return;
        
        foreach (var kutiButton in bindings.keys)
        {
            if(!KutiInput.GetKutiButtonDown(kutiButton)) continue;
            
            RaiseInputEvent(keyCode);
        }
    }

    private void RaiseInputEvent(KeyCode kutiButton)
    {
        bindings[kutiButton].RaiseEvent(true);
    }
    
#else

        private Dictionary<KeyCode, BoolEventChannelSO> bindings;

        private void Awake()
        {
            bindings = new Dictionary<KeyCode, BoolEventChannelSO>
            {
                { KeyCode.A, p1LeftButton },
                { KeyCode.W, p1MiddleButton },
                { KeyCode.D, p1RightButton },
                { KeyCode.H, p2LeftButton },
                { KeyCode.U, p2MiddleButton },
                { KeyCode.K, p2RightButton }
            };
        }

        private void Update()
        {
            if(!Input.anyKeyDown) return;

            foreach (var keyCode in bindings.Keys)
            {
                if(!Input.GetKeyDown(keyCode)) continue;
            
                RaiseInputEvent(keyCode);
                return;
            }
        }

        private void RaiseInputEvent(KeyCode keyCode)
        {
            bindings[keyCode].RaiseEvent(true);
        }
    
#endif
    }
}
