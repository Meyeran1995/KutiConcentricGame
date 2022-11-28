using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/Item to Score Matcher")]
    public class ItemToScoreMatcher : ScriptableObject
    {
        [Header("Sprite matching")]
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private int[] multipliers;
        
        [Header("Color matching")]
        [SerializeField] private Color[] colors;
        [SerializeField] private float[] scores;

        private static readonly Dictionary<Color, float> ColorToScore = new();
        private static readonly Dictionary<Sprite, int> SpriteToMultiplier = new();

        public Color GetRandomColor()
        {
            var colorKeys = ColorToScore.Keys;

            return colorKeys.ElementAt(Random.Range(0, colorKeys.Count));
        }
        
        public float GetScore(SpriteRenderer spriteRenderer)
        {
            
#if UNITY_EDITOR
            if (!ColorToScore.ContainsKey(spriteRenderer.color))
            {
                Debug.LogError("Color is not registered");
                return 10;
            }

            if (!SpriteToMultiplier.ContainsKey(spriteRenderer.sprite))
            {
                Debug.LogError("Sprite is not registered");
                return 10;
            }
#endif
            
            return ColorToScore[spriteRenderer.color] * SpriteToMultiplier[spriteRenderer.sprite];
        }

        private void OnEnable()
        {
            for (int i = 0; i < colors.Length; i++)
            {
                ColorToScore.TryAdd(colors[i], scores[i]);
            }
            
            for (int i = 0; i < sprites.Length; i++)
            {
                SpriteToMultiplier.TryAdd(sprites[i], multipliers[i]);
            }
        }
    }
}