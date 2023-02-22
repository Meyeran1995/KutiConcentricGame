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
        public bool HasPowerUp { get; private set; }

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
            
            MovementData = movementTemplate.GetComponent<SplineContainer>().Spline.ToArray();
            
            var collectiblesRoot = itemTemplate.transform.GetChild(3);
            
            HasPowerUp = collectiblesRoot.TryGetComponent(out PowerUp powerUp) && powerUp.Effect != null;
            
            if(!HasPowerUp) return;
            
            PowerUpData = powerUp.Effect;
        }
    }
}