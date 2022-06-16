using UnityEngine;

namespace Meyham.EditorHelpers
{
    public class DrawGizmoRectangle : MonoBehaviour
    {
        [SerializeField] private Camera main;
        [SerializeField] private Vector2 resolution;
        [SerializeField] private bool drawOnlyWhenSelected;

        private const float refWidth = 1280f;
        private const float refHeight = 1024f;
        
        private float ScreenHeight => 2f * main.orthographicSize;
        private float ScreenWidth => ScreenHeight * main.aspect;

        private void OnDrawGizmos()
        {
            if(drawOnlyWhenSelected || main == null) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, GetSize());
        }
 
        private void OnDrawGizmosSelected()
        {
            if(main == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, GetSize());
        }

        private Vector2 GetSize()
        {
            float x = resolution.x / refWidth * ScreenWidth;
            float y = resolution.y / refHeight * ScreenHeight;

            return new Vector2(x, y);
        }

        private void OnValidate()
        {
            gameObject.name = $"{resolution.x}x{resolution.y}";
        }
    }
}
