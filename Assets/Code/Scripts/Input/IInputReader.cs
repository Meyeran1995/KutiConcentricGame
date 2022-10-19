using System.Collections.Generic;

namespace Meyham.Input
{
    public interface IInputReader
    {
        protected const int P1LeftButton = 0;
        protected const int P1MiddleButton = 1;
        protected const int P1RightButton = 2;
        protected const int P2LeftButton = 3;
        protected const int P2MiddleButton = 4;
        protected const int P2RightButton = 5;
        
        public bool AnyKeyDown();

        public bool AnyKeyUp();

        public IEnumerable<int> GetKeysDown();

        public IEnumerable<int> GetKeysUp();
    }
}