using System.Collections.Generic;
using Meyham.Player;

namespace Meyham.Input
{
    public interface IInputReader
    {
        public bool AnyKeyDown();

        public bool AnyKeyUp();

        public bool KeyHeld(int key);

        public IEnumerable<int> GetKeysDown();

        public IEnumerable<int> GetKeysUp();
    }
}