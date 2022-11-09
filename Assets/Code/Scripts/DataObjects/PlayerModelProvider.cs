using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/PlayerModelProvider")]
    public class PlayerModelProvider : ScriptableObject
    {
        [SerializeField] private Sprite[] playerModels;

        public Sprite GetModel(int order) => playerModels[order];
    }
}
