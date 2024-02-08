using DG.Tweening;
using Meyham.DataObjects;
using UnityEngine;
using static DG.Tweening.DOTweenCYInstruction;

namespace Meyham.Items
{
    public class ItemTweeningAnimation : MonoBehaviour
    {
        [SerializeField] private FloatParameter growTime;
        [SerializeField] private FloatParameter shrinkTime;
        [SerializeField] private FloatParameter timeToTarget;
        
        public WaitForCompletion TweenShrink()
        {
            var tween = transform.DOScale(Vector3.zero, shrinkTime);
            return new WaitForCompletion(tween);
        }

        public WaitForCompletion TweenCollection(Transform targetTransform)
        {
            var parent = transform.parent;
            parent.SetParent(targetTransform, true);
            
            var sequence = DOTween.Sequence();
            sequence.Append(parent.DOLocalMove(Vector3.zero, timeToTarget));
            sequence.Insert(timeToTarget - shrinkTime,transform.DOScale(Vector3.zero, shrinkTime));

            return new WaitForCompletion(sequence);
        }
        
        private void OnEnable()
        {
            transform.DOScale(Vector3.one, growTime);
        }
    }
}
