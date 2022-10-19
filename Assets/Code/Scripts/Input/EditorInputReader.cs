using System.Collections.Generic;
using UnityEngine;

namespace Meyham.Input
{
    public class EditorInputReader : IInputReader
    {
        private readonly Dictionary<KeyCode, int> bindings;
        private readonly List<int> keysDown, keysUp;
        private bool keysUpPopulated;

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

            keysDown = new List<int>(bindings.Count);
            keysUp = new List<int>(bindings.Count);
        }

        public bool AnyKeyDown() => UnityEngine.Input.anyKeyDown;

        public bool AnyKeyUp()
        {
            if (!keysUpPopulated)
            {
                PopulateKeysUp();
            }
            
            return keysUp.Count > 0;
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
            if (keysUpPopulated)
            {
                keysUpPopulated = false;
                return keysUp;
            }

            PopulateKeysUp();

            return keysUp;
        }

        private void PopulateKeysUp()
        {
            keysUpPopulated = true;
            keysUp.Clear();

            foreach (var key in bindings.Keys)
            {
                if(!UnityEngine.Input.GetKeyUp(key)) continue;
                 
                keysUp.Add(bindings[key]);
            }
        }
    }
}