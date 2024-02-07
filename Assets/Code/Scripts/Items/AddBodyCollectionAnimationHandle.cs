using System.Collections;
using UnityEngine;

namespace Meyham.Items
{
    public class AddBodyCollectionAnimationHandle : IEnumerator
    {
        public bool ReleaseAfterPlaying;
        
        private CustomYieldInstruction yieldInstruction;
        
        private bool isPlaying;
        
        private readonly ItemTweeningAnimation tween;
        
        public AddBodyCollectionAnimationHandle(ItemTweeningAnimation itemTween)
        {
            tween = itemTween;
        }

        public void Play(Vector3 endPosition)
        {
            
#if UNITY_EDITOR || DEBUG
            if (isPlaying)
            {
                Debug.LogError("Collection animation was played twice");
                return;
            }
#endif
            
            yieldInstruction = tween.TweenCollection(endPosition);
            isPlaying = true;
        }

        public bool IsPlaying() => isPlaying;

        public bool MoveNext() => yieldInstruction.MoveNext();

        public void Reset() => yieldInstruction.Reset();

        public object Current => yieldInstruction.Current;
    }
}
