
using System.Collections;

namespace Meyham.Animation
{
    public abstract class ATweenBasedAnimation : IEnumerator
    {
        public abstract bool MoveNext();

        public abstract void Cancel();

        public void Reset()
        {
        }

        public object Current => null;
    }
}