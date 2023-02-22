using System.Collections.Generic;
using Meyham.Player;

namespace Meyham.Input
{
    public interface IInputReader
    {
        protected const int P1LeftButton = (int)PlayerDesignation.Orange;
        protected const int P1MiddleButton = (int)PlayerDesignation.Green;
        protected const int P1RightButton = (int)PlayerDesignation.Purple;
        protected const int P2LeftButton = (int)PlayerDesignation.Yellow;
        protected const int P2MiddleButton = (int)PlayerDesignation.Red;
        protected const int P2RightButton = (int)PlayerDesignation.Cyan;
        
        public bool AnyKeyDown();

        public bool AnyKeyUp();

        public IEnumerable<int> GetKeysDown();

        public IEnumerable<int> GetKeysUp();
    }
}