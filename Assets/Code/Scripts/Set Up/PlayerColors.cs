using UnityEngine;

namespace Meyham.Set_Up
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/PlayerColors")]
    public class PlayerColors : ScriptableObject
    {
        [SerializeField] private Color[] colors;

        public Color this[int i] => colors[i];
    }
}