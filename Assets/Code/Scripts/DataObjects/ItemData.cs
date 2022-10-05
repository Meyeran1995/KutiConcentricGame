using Meyham.EditorHelpers;
using Meyham.Items;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/ItemData")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private GameObject itemTemplate, movementTemplate;

        [field: Header("Debug"), SerializeField, ReadOnly]
        public Color Color { get; private set; }

        [field: SerializeField, ReadOnly]
        public BezierKnot[] MovementData { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public float ScoreData { get; private set; }

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
            Color = itemTemplate.GetComponent<SpriteRenderer>().color;
            
            var collisionTransform = itemTemplate.transform.GetChild(0);

            MovementData = movementTemplate.GetComponent<SplineContainer>().Spline.ToArray();
            ScoreData = collisionTransform.GetComponent<AddScoreCollectible>().Score;

            if (!collisionTransform.TryGetComponent(out PowerUp powerUp)) return;
            
            IsPowerUp = true;
            PowerUpData = powerUp.Effect;
        }
    }
}