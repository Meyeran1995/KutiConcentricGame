using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Meyham.Events
{
    /// <summary>
    /// Globally raises events based on input received
    /// </summary>
    public class InputEventDistributor : MonoBehaviour
    {
        [Header("Inputs")] 
        [SerializeField] private IntEventChannelSO inputEventChannel;

        private const int P1LeftButton = 0;
        private const int P1MiddleButton = 1;
        private const int P1RightButton = 2;
        private const int P2LeftButton = 3;
        private const int P2MiddleButton = 4;
        private const int P2RightButton = 5;

#if UNITY_ANDROID
    
    private Dictionary<EKutiButton, int> bindings;

    private void Awake()
    {
        bindings = new Dictionary<EKutiButton, int>
        {
            { EKutiButton.P1_LEFT, P1LeftButton },
            { EKutiButton.P1_MID, P1MiddleButton },
            { EKutiButton.P1_RIGHT, P1RightButton },
            { EKutiButton.P2_LEFT, P2LeftButton },
            { EKutiButton.P2_MID, P2MiddleButton },
            { EKutiButton.P2_RIGHT, P2RightButton }
        };
    }


    private void Update()
    {
        if(!Input.anyKeyDown) return;
        
        foreach (var kutiButton in bindings.Keys)
        {
            if(!KutiInput.GetKutiButtonDown(kutiButton)) continue;
            
            RaiseInputEvent(kutiButton);
        }
    }

    private void RaiseInputEvent(EKutiButton kutiButton)
    {
        inputEventChannel.RaiseEvent(bindings[kutiButton]);
    }
    
#else

        private Dictionary<KeyCode, int> bindings;

        private void Awake()
        {
            bindings = new Dictionary<KeyCode, int>
            {
                { KeyCode.A, P1LeftButton },
                { KeyCode.W, P1MiddleButton },
                { KeyCode.D, P1RightButton },
                { KeyCode.H, P2LeftButton },
                { KeyCode.U, P2MiddleButton },
                { KeyCode.K, P2RightButton }
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
            inputEventChannel.RaiseEvent(bindings[keyCode]);
        }
    
#endif
    }
}
