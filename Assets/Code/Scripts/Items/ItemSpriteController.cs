using System;
using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemSpriteController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ItemToScoreMatcher toScore;
        [SerializeField] private AddScoreCollectible scoring;
        [SerializeField] private ItemTweeningAnimation tweeningAnimation;
        
        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            
            spriteRenderer.color = toScore.GetRandomColor();
            scoring.SetScore(toScore.GetScore(spriteRenderer));
            
            spriteRenderer.transform.localScale = Vector3.zero;
        }

        private void OnEnable()
        {
            tweeningAnimation.enabled = true;
        }
        
        private void OnDisable()
        {
            tweeningAnimation.enabled = false;
        }
    }
}