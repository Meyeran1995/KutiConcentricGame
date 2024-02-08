using DG.Tweening;
using Meyham.DataObjects;
using UnityEngine;
using UnityEngine.U2D;

namespace Meyham.Player.Bodies
{
    public class BodyTweenAnimation : MonoBehaviour
    {
        [Header("")]
        [SerializeField] private FloatParameter fadeValue;
        [SerializeField] private FloatParameter fadeDuration;
        
        [Header("References")]
        [SerializeField] private SpriteShapeRenderer shapeRenderer;

        public float GetDuration()
        {
            return fadeDuration * 1.5f;
        }
        
        public Sequence FlickerAnimation()
        {
            var tween = DOTween.ToAlpha(() => shapeRenderer.color, color => shapeRenderer.color = color, fadeValue,fadeDuration);
            
            var sequence = DOTween.Sequence();
            sequence.Append(tween);
            
            tween = DOTween.ToAlpha(() => shapeRenderer.color, color => shapeRenderer.color = color, 1f,fadeDuration / 2f);

            sequence.Append(tween);

            return sequence;
        }
        
    }
}
