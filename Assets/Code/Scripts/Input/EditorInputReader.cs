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
                { KeyCode.A, IInputReader.P1LeftButton },
                { KeyCode.W, IInputReader.P1MiddleButton },
                { KeyCode.D, IInputReader.P1RightButton },
                { KeyCode.H, IInputReader.P2LeftButton },
                { KeyCode.U, IInputReader.P2MiddleButton },
                { KeyCode.K, IInputReader.P2RightButton }
            };
            
            keysByBindings = new Dictionary<int, KeyCode>
            {
                { IInputReader.P1LeftButton, KeyCode.A },
                { IInputReader.P1MiddleButton, KeyCode.W },
                { IInputReader.P1RightButton, KeyCode.D },
                { IInputReader.P2LeftButton, KeyCode.H },
                { IInputReader.P2MiddleButton, KeyCode.U },
                { IInputReader.P2RightButton, KeyCode.K }
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