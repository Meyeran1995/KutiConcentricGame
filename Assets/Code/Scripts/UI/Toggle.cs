using UnityEngine;
using UnityEngine.UI;

namespace Meyham.UI
{
    public class Toggle : MonoBehaviour
    {
        [SerializeField] private Sprite toggled;

        private Sprite unToggled;
        private Image imageRenderer;
        
        private bool isToggled;
        
        public bool IsToggled
        {
            get => isToggled;
            set
            {
                isToggled = value;
                imageRenderer.sprite = isToggled ? toggled : unToggled;
            }
        }

        private void Awake()
        {
            imageRenderer = GetComponent<Image>();
            unToggled = imageRenderer.sprite;
        }
    }
}
