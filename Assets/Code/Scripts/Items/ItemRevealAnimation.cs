using DG.Tweening;
using Meyham.DataObjects;
using UnityEngine;

namespace Meyham.Items
{
    public class ItemRevealAnimation : MonoBehaviour
    {
        [SerializeField] private float minimalScale;
        [SerializeField] private FloatValue revealTime;
        
        private Vector3 originalScale;
        private Tweener tweener;
        
        private void Awake()
        {
            var itemTransform = transform;
            originalScale = itemTransform.localScale;
            itemTransform.localScale = new Vector3(minimalScale, minimalScale, 1f);
        }

        private void OnDisable()
        {
            transform.localScale = new Vector3(minimalScale, minimalScale, 1f);
            
            if(!tweener.active) return;

            tweener.Kill();
            tweener = null;
        }

        private void OnEnable()
        {
            tweener = transform.DOScale(originalScale, revealTime);
        }
    }
}
