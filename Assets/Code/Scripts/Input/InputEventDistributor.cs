using System;
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

        private readonly Dictionary<int, int> tapCountPerKey = new(2);
        
        private readonly Dictionary<int, bool> isHeldPerKey = new(2);

        private void Awake()
        {
#if UNITY_EDITOR
            inputReader = new EditorInputReader();
#else
            inputReader = new KutiInputReader();
#endif
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
                if (!isHeldPerKey.ContainsKey(key) || !isHeldPerKey[key])
                {
                    continue;
                }
                
                isHeldPerKey[key] = false;
                inputCanceledEventChannel.RaiseEvent(key);
            }
            
            foreach (var key in inputReader.GetKeysDown())
            {
                StartCoroutine(WaitForHold(key));
            }
        }

        private void InputWithoutHold()
        {
            foreach (var key in inputReader.GetKeysDown())
            {
                inputEventChannel.RaiseEvent(key);
            }
        }

        private IEnumerator WaitForHold(int key)
        {
            float timer = 0f;

            if (!tapCountPerKey.TryAdd(key, 1) && tapCountPerKey[key] == 0)
            {
                tapCountPerKey[key] = 1;
            }
            
            while (inputReader.KeyHeld(key) && timer < holdThreshold)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (inputReader.KeyHeld(key))
            {
                inputEventChannel.RaiseEvent(key);
                isHeldPerKey[key] = true;
                yield break;
            }
            
            OnDoubleTap(key);
        }

        private void OnDoubleTap(int key)
        {
            var tapCount = tapCountPerKey[key];
            
            if (tapCount == 2)
            {
                inputDoubleTapEventChannel.RaiseEvent(key);
                tapCountPerKey[key] = 0;
                return;
            }

            tapCountPerKey[key] = tapCount + 1;
        }
    }
}
