using UnityEditor;
using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/ItemWave")]
    public class CollectibleWave : ScriptableObject
    {
        [Header("Values")]
        [SerializeField] private ItemData[] itemTypes;
        [SerializeField] private int[] contents;
        [Header("Randomness")]
        [SerializeField] private bool randomize;
        [SerializeField] private float[] chances;
        
        public ItemData[] Wave { get; private set; }

        private void OnEnable()
        {
            Wave = new ItemData[contents.Length];

            if (randomize)
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    Wave[i] = GetRandomItem();
                }
                return;
            }
            
            for (int i = 0; i < contents.Length; i++)
            {
                Wave[i] = itemTypes[contents[i]];
            }
        }

        private void OnValidate()
        {
            var numOfItemTypes = itemTypes.Length;
            var numOfItems = contents.Length;

            for (int i = 0; i < numOfItems; i++)
            {
                int content = contents[i];

                if (content < 0)
                    content = -content;

                contents[i] = content % numOfItemTypes;
            }

            if (!randomize)
            {
                chances = null;
                return;
            }

            chances = new float[numOfItemTypes];

            for (int i = 0; i < numOfItemTypes; i++)
            {
                float count = 0f;
                
                for (int c = 0; c < numOfItems; c++)
                {
                    if(contents[c] != i) continue;
                    count++;
                }

                chances[i] = count / numOfItems;
            }
        }

        private ItemData GetRandomItem()
        {
            var chance = Random.Range(0f, 1f);
            var item = itemTypes[^1];

            for (int i = 0; i < chances.Length; i++)
            {
                if(chance > chances[i]) continue;
                return itemTypes[i];
            }

            return item;
        }
    }
}