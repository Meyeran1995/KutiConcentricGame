﻿using Meyham.EditorHelpers;
using Meyham.Items;
using Meyham.Splines;
using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Serialized Data")]
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private GameObject movementTemplate;
        
        [field: SerializeField]
        public CurveParameter SpeedCurve { get; private set; }

        [field: Header("Debug"), SerializeField, ReadOnly]
        public Sprite Sprite { get; private set; }

        [field: SerializeField, ReadOnly] 
        public Color Color { get; private set; }

        [field: SerializeField, ReadOnly]
        public Vector3 ColliderPosition { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public Quaternion ColliderRotation { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public Vector3 ColliderScale { get; private set; }
        
        public SplineKnotData MovementData { get; private set; }
        
        public ACollectibleData CollectibleData { get; private set; }

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
            Color = renderer.color;

            var spline = movementTemplate.GetComponent<SplineContainer>().Spline;
            var tangentModes = new TangentMode[spline.Count];

            for (int i = 0; i < tangentModes.Length; i++)
            {
                tangentModes[i] = spline.GetTangentMode(i);
            }
            
            MovementData = new SplineKnotData(spline.ToArray(), tangentModes);
            
            var itemCollisionTransform = itemTemplate.GetComponentInChildren<ItemCollision>().transform;

            ColliderPosition = itemCollisionTransform.localPosition;
            ColliderRotation = itemCollisionTransform.localRotation;
            ColliderScale = itemCollisionTransform.localScale;

            CollectibleData = itemCollisionTransform.GetComponent<ItemCollectibleCarrier>().Collectible;
        }
    }
}