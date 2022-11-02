using UnityEngine;
using UnityEngine.UI;

namespace Meyham.UI
{
    public class Toggle : MonoBehaviour
    {
        [SerializeField] private Image imageRenderer;
        
        [SerializeField] private Sprite toggled;

        private Sprite unToggled;

        private bool isToggled;
        
        public void ToggleImage()
        {
            isToggled = !isToggled;
            imageRenderer.sprite = isToggled ? toggled : unToggled;
        }

        private void Awake()
        {
            unToggled = imageRenderer.sprite;
        }
    }
}
