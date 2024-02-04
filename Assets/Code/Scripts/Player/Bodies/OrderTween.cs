using DG.Tweening;
using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player.Bodies
{
    public class OrderTween : MonoBehaviour
    {
        private const float angle = 22.5f;
        
        [SerializeField] private FloatParameter tweenSpeed;

        private Sequence activeSequence;

        private int lastOrder;
        
        public YieldInstruction TweenToPosition(Vector3 localPosition, int order)
        {
            float currentAngle = order >= lastOrder ? -angle : angle;
            lastOrder = order;
            
            activeSequence = DOTween.Sequence();
            activeSequence.Append(transform.DOLocalMove(localPosition, tweenSpeed));
            activeSequence.Insert(0,transform.DOLocalRotate(new Vector3(0f, 0f, currentAngle), tweenSpeed));
            activeSequence.Append(transform.DOLocalRotate(Vector3.zero, tweenSpeed / 2f));
            
            return activeSequence.WaitForCompletion();
        }

        private void OnDisable()
        {
            lastOrder = 0;
            
            if (activeSequence is not { active: true }) return;
            
            activeSequence.Kill();
        }
    }
}
