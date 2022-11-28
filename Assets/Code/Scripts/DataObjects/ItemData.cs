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
        public APowerUpEffect PowerUpData { get; private set; }

        [field: SerializeField, ReadOnly]
        public bool IsPowerUp { get; private set; }

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
            var renderer = itemTemplate.transform.GetChild(2).GetComponent<SpriteRenderer>();
            Sprite = renderer.sprite;
            
            var collisionTransform = itemTemplate.transform.GetChild(0);

            MovementData = movementTemplate.GetComponent<SplineContainer>().Spline.ToArray();

            if (!collisionTransform.TryGetComponent(out PowerUp powerUp)) return;
            
            IsPowerUp = true;
            PowerUpData = powerUp.Effect;
        }
    }
}