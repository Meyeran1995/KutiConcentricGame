using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemSpriteController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ItemToScoreMatcher toScore;
        [SerializeField] private AddScoreCollectible scoring;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            
            spriteRenderer.color = toScore.GetRandomColor();
            scoring.SetScore(toScore.GetScore(this.spriteRenderer));
        }
    }
}