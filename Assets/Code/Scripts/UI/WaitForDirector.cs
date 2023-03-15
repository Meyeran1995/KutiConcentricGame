using System.Collections;
using UnityEngine.Playables;

namespace Meyham.UI
{
    public class WaitForDirector : IEnumerator
    {
        private PlayableDirector director;
        
        public WaitForDirector(PlayableDirector director)
        {
            this.director = director;
        }

        public object Current => null;

        public bool MoveNext() => director.state == PlayState.Playing;

        public void Reset()
        {
        }
    }
}