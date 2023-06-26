using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Templates")]
        [SerializeField] private GameObject itemTemplate, movementTemplate;

        [field: Header("Debug"), SerializeField, ReadOnly]
        public Sprite Sprite { get; private set; }

        [field: SerializeField, ReadOnly]
        public BezierKnot[] MovementData { get; private set; }

        private void OnEnable()
        {
            GetDataFromTemplates();
        }

        private void OnValidate()
        {
            if(!itemTemplate || !movementTemplate) return;
            
            GetDataFromTemplates();
        }

        private void GetDataFromTemplates()
        {
            var renderer = itemTemplate.transform.GetComponentInChildren<SpriteRenderer>();
            Sprite = renderer.sprite;
            
            MovementData = movementTemplate.GetComponent<SplineContainer>().Spline.ToArray();
        }
    }
}