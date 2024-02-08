
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
            var durationIncrement = bodyTweenAnimations[0].GetDuration() / 2f;
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
            return !hasStartedPlaying || (animationSequence.active && !animationSequence.IsComplete());
        }
    }
}