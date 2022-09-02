using DG.Tweening;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemSizeController : MonoBehaviour
    {
        [field: Header("Size"), SerializeField, Min(0f)] public float MaxSize { get; private set; }
        [field: SerializeField, Min(0f)] public float TimeToMaxSize{ get; private set; }

        private float originalScale;
        private Tweener tweener;
        
        public void StartGainingSize()
        {
            tweener = transform.DOScale(new Vector3(MaxSize, MaxSize, MaxSize), TimeToMaxSize);
        }

        public void ResetSize()
        {
            transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }
        
        private void Awake()
        {
            originalScale = transform.localScale.x;
        }

        private void OnEnable()
        {
            StartGainingSize();
        }

        private void OnDisable()
        {
            if(tweener.active)
                tweener.Kill();
            ResetSize();
        }

#if UNITY_EDITOR

        [Header("Debug")]
        [SerializeField] private bool showGizmos;
        [SerializeField] private Vector2 gizmoEnd;
        
        private void OnDrawGizmosSelected()
        {
            if(!showGizmos) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawCube(gizmoEnd, Vector3.one * MaxSize);
        }

#endif
    }
}
