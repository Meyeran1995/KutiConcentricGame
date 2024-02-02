using UnityEngine;
using UnityEngine.U2D;

namespace Meyham.Player.Bodies
{
    public class DummyBodyPart : MonoBehaviour
    {
        [SerializeField] private SpriteShapeRenderer spriteRenderer;
        
        public void SetColor(Color activeColor)
        {
            spriteRenderer.color = activeColor;
        }
        
        public void Show()
        {
            spriteRenderer.enabled = true;
        }
        
        public void Hide()
        {
            spriteRenderer.enabled = false;
        }
    }
}
