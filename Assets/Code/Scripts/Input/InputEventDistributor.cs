using System.Collections;
using System.Collections.Generic;
using Meyham.Events;
using UnityEngine;

namespace Meyham.Input
{
    /// <summary>
    /// Globally raises events based on input received
    /// </summary>
    public class InputEventDistributor : MonoBehaviour
    {
        [Header("Hold properties")] 
        [SerializeField] private float holdThreshold;
        
        [Header("Inputs")] 
        [SerializeField] private IntEventChannelSO inputEventChannel;
        [SerializeField] private IntEventChannelSO inputDoubleTapEventChannel;
        [SerializeField] private IntEventChannelSO inputCanceledEventChannel;
        
        [Header("Events")]
        [SerializeField] private GenericEventChannelSO<bool> setHoldInteractionEventChannel;
        
        private bool enableHold;

        private IInputReader inputReader;

        private Dictionary<int, KeyInfo> infoPerKey;

        private void Awake()
        {
#if UNITY_EDITOR
            inputReader = new EditorInputReader();
#else
            inputReader = new KutiInputReader();
#endif

            infoPerKey = new Dictionary<int, KeyInfo>()
            {
                {InputConstants.P1LeftButton, new KeyInfo(InputConstants.P1LeftButton)},
                {InputConstants.P1MiddleButton, new KeyInfo(InputConstants.P1MiddleButton)},
                {InputConstants.P1RightButton, new KeyInfo(InputConstants.P1RightButton)},
                {InputConstants.P2LeftButton, new KeyInfo(InputConstants.P2LeftButton)},
                {InputConstants.P2MiddleButton, new KeyInfo(InputConstants.P2MiddleButton)},
                {InputConstants.P2RightButton, new KeyInfo(InputConstants.P2RightButton)}
            };
        }

        private void OnEnable()
        {
            setHoldInteractionEventChannel += SetHoldInteraction;
        }

        private void Update()
        {
            if (!inputReader.AnyKeyUp() && !inputReader.AnyKeyDown()) return;

            if (enableHold)
            {
                InputWithHold();
                return;
            }

            InputWithoutHold();
        }

        private void SetHoldInteraction(bool enable)
        {
            enableHold = enable;
        }
        
        private void InputWithHold()
        {
            foreach (var key in inputReader.GetKeysUp())
            {
                var keyInfo = infoPerKey[key];
                if (!keyInfo.IsHeld)
                {
                    continue;
                }
                
                keyInfo.IsHeld = false;
                inputCanceledEventChannel.RaiseEvent(key);
            }
            
            foreach (var key in inputReader.GetKeysDown())
            {
                var keyInfo = infoPerKey[key];

                OnButtonTap(keyInfo);
                
                if (keyInfo.HoldRoutineIsActive)
                {
                    continue;
                }
                
                StartCoroutine(WaitForHold(keyInfo));
            }
        }

        private void InputWithoutHold()
        {
            foreach (var key in inputReader.GetKeysDown())
            {
                inputEventChannel.RaiseEvent(key);
            }
        }

        private IEnumerator WaitForHold(KeyInfo keyInfo)
        {
            float timer = 0f;
            keyInfo.HoldRoutineIsActive = true;

            while (timer < holdThreshold && inputReader.KeyHeld(keyInfo.ID))
            {
                timer += Time.deltaTime;
                yield return null;
            }

            keyInfo.HoldRoutineIsActive = false;
            
            if (!inputReader.KeyHeld(keyInfo.ID))
            {
                yield break;
            }
            
            Debug.Log("Hold");
            inputEventChannel.RaiseEvent(keyInfo.ID);
            
            keyInfo.IsHeld = true;
            keyInfo.HoldRoutineIsActive = false;
            keyInfo.TapCount = 0;
            Debug.Log("Tap count reset");
        }

        private void OnButtonTap(KeyInfo keyInfo)
        {
            keyInfo.TapCount++;
            Debug.Log($"Tap {keyInfo.TapCount}");

            if (keyInfo.TapCount < 2) return;

            inputDoubleTapEventChannel.RaiseEvent(keyInfo.ID);
            keyInfo.TapCount = 0;
        }
        
        private class KeyInfo
        {
            public bool IsHeld;
            public bool HoldRoutineIsActive;
            public int TapCount;
            public int ID { get; }

            public KeyInfo(int id)
            {
                ID = id;
            }
        }
    }
}
