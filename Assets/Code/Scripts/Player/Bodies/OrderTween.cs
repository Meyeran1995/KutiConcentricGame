using System;
using DG.Tweening;
using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Player.Bodies
{
    public class OrderTween : MonoBehaviour
    {
        [SerializeField] private FloatParameter tweenSpeed;

        private Sequence activeSequence;
        
        public YieldInstruction TweenToPosition(Vector3 localPosition)
        {
            activeSequence = DOTween.Sequence();
            activeSequence.Append(transform.DOLocalMove(localPosition, tweenSpeed));
            activeSequence.Insert(0,transform.DOLocalRotate(new Vector3(0f, 0f, 22.5f), tweenSpeed));
            activeSequence.Append(transform.DOLocalRotate(Vector3.zero, tweenSpeed / 2f));

            return activeSequence.WaitForCompletion();
        }

        private void OnDisable()
        {
            if (activeSequence is not { active: true }) return;
            
            activeSequence.Kill();
        }
    }
}
