using Meyham.Events;
using UnityEngine;

namespace Meyham.Input
{
    /// <summary>
    /// Globally raises events based on input received
    /// </summary>
    public class InputEventDistributor : MonoBehaviour
    {
        [Header("Inputs")] 
        [SerializeField] private IntEventChannelSO inputEventChannel;
        [SerializeField] private IntEventChannelSO inputCanceledEventChannel;

        private IInputReader inputReader;
        
        private void Awake()
        {
#if UNITY_EDITOR
            inputReader = new EditorInputReader();
#else
            inputReader = new KutiInputReader();
#endif
        }

        private void Update()
        {
            if (!inputReader.AnyKeyUp() && !inputReader.AnyKeyDown()) return;
            
            foreach (var key in inputReader.GetKeysUp())
            {
                inputCanceledEventChannel.RaiseEvent(key);
            }
            
            foreach (var key in inputReader.GetKeysDown())
            {
                inputEventChannel.RaiseEvent(key);
            }
        }
    }
}
