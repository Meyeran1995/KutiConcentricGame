using UnityEngine;
using UnityEngine.Splines;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/StatObjects/ItemWave")]
    public class CollectibleWave : ScriptableObject
    {
        [SerializeField] private GameObject[] itemTypes;
        [SerializeField] private int[] contents;
        
        public BezierKnot[][] Wave { get; private set; }

        private void OnEnable()
        {
            Wave = new BezierKnot[contents.Length][];

            var splines = new SplineContainer[itemTypes.Length];

            for (int i = 0; i < itemTypes.Length; i++)
            {
                splines[i] = itemTypes[i].GetComponent<SplineContainer>();
            }
            
            for (int i = 0; i < contents.Length; i++)
            {
                Wave[i] = splines[contents[i]].Spline.ToArray();
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