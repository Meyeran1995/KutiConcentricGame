using UnityEngine;

namespace Meyham.Items
{
    public class ItemSpriteController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ItemTweeningAnimation tweeningAnimation;
        
        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.transform.localScale = Vector3.zero;
        }

        public void SetMaterial(Material itemMaterial)
        {
            spriteRenderer.sharedMaterial = itemMaterial;
        }

        public void SetColor(Color color)
        {
            spriteRenderer.color = color;
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