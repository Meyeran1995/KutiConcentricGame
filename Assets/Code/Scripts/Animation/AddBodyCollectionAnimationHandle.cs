using Meyham.Items;
using UnityEngine;

namespace Meyham.Animation
{
    public class AddBodyCollectionAnimationHandle : ATweenBasedAnimation
    {
        public bool ReleaseAfterPlaying;
        
        private bool hasStartedPlaying;
        
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
            return !hasStartedPlaying || yieldInstruction.MoveNext();
        }
    }
}
