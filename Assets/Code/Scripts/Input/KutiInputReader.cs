using System.Collections.Generic;

namespace Meyham.Input
{
    public class KutiInputReader : IInputReader
    {
        private readonly Dictionary<EKutiButton, int> bindings;
        private readonly List<int> keysDown, keysUp;

        public KutiInputReader()
        {
            bindings = new Dictionary<EKutiButton, int>
            {
                { EKutiButton.P1_LEFT, IInputReader.P1LeftButton },
                { EKutiButton.P1_MID, IInputReader.P1MiddleButton },
                { EKutiButton.P1_RIGHT, IInputReader.P1RightButton },
                { EKutiButton.P2_LEFT, IInputReader.P2LeftButton },
                { EKutiButton.P2_MID, IInputReader.P2MiddleButton },
                { EKutiButton.P2_RIGHT, IInputReader.P2RightButton }
            };

            keysDown = new List<int>(bindings.Count);
            keysUp = new List<int>(bindings.Count);
        }

        public bool AnyKeyDown() => KutiInput.GetAnyButtonDown();

        public bool AnyKeyUp()
        {
            PopulateKeysUp();
            
            return keysUp.Count > 0;
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