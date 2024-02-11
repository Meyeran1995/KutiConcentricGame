
using System.Collections.Generic;
using DG.Tweening;
using Meyham.Player.Bodies;

namespace Meyham.Animation
{
    public class DestroyBodyCollectionHandle : ATweenBasedAnimation
    {
        public BodyPart Origin { get; }

        private readonly List<BodyTweenAnimation> bodyTweenAnimations;

        private Sequence animationSequence;
        
        private bool hasStartedPlaying;
        
        private bool wasCanceled;
        
        public DestroyBodyCollectionHandle(BodyPart bodyPart)
        {
            Origin = bodyPart;
            bodyTweenAnimations = new List<BodyTweenAnimation>(3) { bodyPart.GetTweenAnimation() };
        }

        public void AddBodyPartToAnimation(BodyTweenAnimation bodyTweenAnimation)
        {
            bodyTweenAnimations.Add(bodyTweenAnimation);
        }

        public void Play()
        {
            var durationIncrement = bodyTweenAnimations[0].GetDuration() / bodyTweenAnimations.Count;
            hasStartedPlaying = true;
            
            animationSequence = DOTween.Sequence();
            animationSequence.Append(bodyTweenAnimations[0].FlickerAnimation());
            
            for (int i = 1; i < bodyTweenAnimations.Count; i++)
            {
                animationSequence.Insert(durationIncrement * i, bodyTweenAnimations[i].FlickerAnimation());
            }
        }

        public override bool MoveNext()
        {
            if (wasCanceled)
            {
                return false;
            }
            
            return !hasStartedPlaying || (animationSequence.active && !animationSequence.IsComplete());
        }

        public override void Cancel()
        {
            wasCanceled = true;
            animationSequence?.Kill();
        }
    }
}