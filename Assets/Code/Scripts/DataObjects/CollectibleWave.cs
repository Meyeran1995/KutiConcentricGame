using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/StatObjects/ItemWave")]
    public class CollectibleWave : ScriptableObject
    {
        [SerializeField] private ItemMovementStatsSO[] itemTypes;
        [SerializeField] private int[] contents;
        
        [field: Header("Debug"), ReadOnly, SerializeField] 
        public ItemMovementStatsSO[] Wave { get; private set; }

        private void OnEnable()
        {
            Wave = new ItemMovementStatsSO[contents.Length];
            
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