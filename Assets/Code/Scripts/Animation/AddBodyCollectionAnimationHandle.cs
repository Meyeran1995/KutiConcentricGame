using Meyham.Items;
using UnityEngine;

namespace Meyham.Animation
{
    public class AddBodyCollectionAnimationHandle : ATweenBasedAnimation
    {
        public bool ReleaseBodyAfterPlaying;
        
        private bool hasStartedPlaying;

        private bool wasCanceled;
        
        private CustomYieldInstruction yieldInstruction;

        private readonly ItemTweeningAnimation tween;

        public AddBodyCollectionAnimationHandle(ItemTweeningAnimation itemTween)
        {
            tween = itemTween;
        }

        public void Play(Transform collectionTarget)
        {
            
#if UNITY_EDITOR || DEBUG
            if (hasStartedPlaying)
            {
                Debug.LogError("Collection animation was played twice");
                return;
            }
#endif
            yieldInstruction = tween.TweenCollection(collectionTarget);
            hasStartedPlaying = true;
        }

        public override bool MoveNext()
        {
            if (wasCanceled)
            {
                return false;
            }
            
            return !hasStartedPlaying || yieldInstruction.MoveNext();
        }

        public override void Cancel()
        {
            wasCanceled = true;
            tween.StopAnimation();
        }
    }
}
