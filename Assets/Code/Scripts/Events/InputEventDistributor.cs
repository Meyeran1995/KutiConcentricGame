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
        [FormerlySerializedAs("inputEventChannel")]
        [Header("Inputs")] 
        [SerializeField] private IntEventChannelSO inputEventChannelSo;

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
            inputEventChannelSo.RaiseEvent(bindings[keyCode]);
        }
    
#endif
    }
}
