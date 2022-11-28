using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemSpriteController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteOutline, spriteFill;
        [SerializeField] private ItemToScoreMatcher toScore;
        [SerializeField] private AddScoreCollectible scoring;

        public void SetSprite(Sprite sprite)
        {
            spriteFill.sprite = sprite;
            spriteOutline.sprite = sprite;
            
            spriteFill.color = toScore.GetRandomColor();
            scoring.SetScore(toScore.GetScore(spriteFill));
        }
    }
}