using System.Collections.Generic;
using UnityEngine;

namespace Meyham.Input
{
    public class EditorInputReader : IInputReader
    {
        private readonly Dictionary<KeyCode, int> bindings;
        private readonly Dictionary<int, KeyCode> keysByBindings;
        private readonly List<int> keysDown, keysUp, keysHeld;

        public EditorInputReader()
        {
            bindings = new Dictionary<KeyCode, int>
            {
                { KeyCode.A, InputConstants.P1LeftButton },
                { KeyCode.W, InputConstants.P1MiddleButton },
                { KeyCode.D, InputConstants.P1RightButton },
                { KeyCode.H, InputConstants.P2LeftButton },
                { KeyCode.U, InputConstants.P2MiddleButton },
                { KeyCode.K, InputConstants.P2RightButton }
            };
            
            keysByBindings = new Dictionary<int, KeyCode>
            {
                { InputConstants.P1LeftButton, KeyCode.A },
                { InputConstants.P1MiddleButton, KeyCode.W },
                { InputConstants.P1RightButton, KeyCode.D },
                { InputConstants.P2LeftButton, KeyCode.H },
                { InputConstants.P2MiddleButton, KeyCode.U },
                { InputConstants.P2RightButton, KeyCode.K }
            };

            keysDown = new List<int>(bindings.Count);
            keysUp = new List<int>(bindings.Count);
            keysHeld = new List<int>(bindings.Count);
        }

        public bool AnyKeyDown() => UnityEngine.Input.anyKeyDown;

        public bool AnyKeyUp()
        {
            PopulateKeysUp();
            
            return keysUp.Count > 0;
        }

        public bool KeyHeld(int key)
        {
            return UnityEngine.Input.GetKey(keysByBindings[key]);
        }

        public IEnumerable<int> GetKeysDown()
        {
            keysDown.Clear();
            
            foreach (var key in bindings.Keys)
            {
                if(!UnityEngine.Input.GetKeyDown(key)) continue;
                 
                keysDown.Add(bindings[key]);
            }

            return keysDown;
        }

        public IEnumerable<int> GetKeysUp()
        {
            return keysUp;
        }

        private void PopulateKeysUp()
        {
            keysUp.Clear();

            foreach (var key in bindings.Keys)
            {
                if(!UnityEngine.Input.GetKeyUp(key)) continue;
                 
                keysUp.Add(bindings[key]);
            }
        }
    }
}