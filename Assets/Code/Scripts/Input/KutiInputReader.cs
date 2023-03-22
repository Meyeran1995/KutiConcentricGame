using System.Collections.Generic;

namespace Meyham.Input
{
    public class KutiInputReader : IInputReader
    {
        private readonly Dictionary<EKutiButton, int> bindings;
        private readonly Dictionary<int, EKutiButton> buttonsByBindings;
        private readonly List<int> keysDown, keysUp, keysHeld;

        public KutiInputReader()
        {
            bindings = new Dictionary<EKutiButton, int>
            {
                { EKutiButton.P1_LEFT, InputConstants.P1LeftButton },
                { EKutiButton.P1_MID, InputConstants.P1MiddleButton },
                { EKutiButton.P1_RIGHT, InputConstants.P1RightButton },
                { EKutiButton.P2_LEFT, InputConstants.P2LeftButton },
                { EKutiButton.P2_MID, InputConstants.P2MiddleButton },
                { EKutiButton.P2_RIGHT, InputConstants.P2RightButton }
            };
            
            buttonsByBindings = new Dictionary<int, EKutiButton>
            {
                { InputConstants.P1LeftButton, EKutiButton.P1_LEFT },
                { InputConstants.P1MiddleButton, EKutiButton.P1_MID },
                { InputConstants.P1RightButton, EKutiButton.P1_RIGHT },
                { InputConstants.P2LeftButton, EKutiButton.P2_LEFT },
                { InputConstants.P2MiddleButton, EKutiButton.P2_MID },
                { InputConstants.P2RightButton, EKutiButton.P2_RIGHT }
            };

            keysDown = new List<int>(bindings.Count);
            keysUp = new List<int>(bindings.Count);
            keysHeld = new List<int>(bindings.Count);
        }

        public bool AnyKeyDown() => KutiInput.GetAnyButtonDown();

        public bool AnyKeyUp()
        {
            PopulateKeysUp();
            
            return keysUp.Count > 0;
        }

        public bool KeyHeld(int key)
        {
            return KutiInput.GetKutiButton(buttonsByBindings[key]);
        }

        public IEnumerable<int> GetKeysDown()
        {
            keysDown.Clear();
            
            foreach (var kutiButton in bindings.Keys)
            {
                if(!KutiInput.GetKutiButtonDown(kutiButton)) continue;
                 
                keysDown.Add(bindings[kutiButton]);
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

            foreach (var kutiButton in bindings.Keys)
            {
                if(!KutiInput.GetKutiButtonUp(kutiButton)) continue;
                 
                keysUp.Add(bindings[kutiButton]);
            }
        }
    }
}