using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/ItemWave")]
    public class CollectibleWave : ScriptableObject
    {
        [SerializeField] private ItemData[] itemTypes;
        [SerializeField] private int[] contents;
        
        public ItemData[] Wave { get; private set; }

        private void OnEnable()
        {
            Wave = new ItemData[contents.Length];
            
            for (int i = 0; i < contents.Length; i++)
            {
                Wave[i] = itemTypes[contents[i]];
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < contents.Length; i++)
            {
                int content = contents[i];

                if (content < 0)
                    content = -content;

                contents[i] = content % itemTypes.Length;
            }
        }
    }
}