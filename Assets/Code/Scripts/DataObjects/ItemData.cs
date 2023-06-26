using Meyham.EditorHelpers;
using Meyham.Items;
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
        
        [field: SerializeField, ReadOnly]
        public Vector3 ColliderPosition { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public Quaternion ColliderRotation { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public Vector3 ColliderScale { get; private set; }

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

            var itemCollisionTransform = itemTemplate.GetComponentInChildren<ItemCollision>().transform;

            ColliderPosition = itemCollisionTransform.localPosition;
            ColliderRotation = itemCollisionTransform.localRotation;
            ColliderScale = itemCollisionTransform.localScale;
        }
    }
}