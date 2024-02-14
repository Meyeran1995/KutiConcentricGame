using Meyham.Player;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.DataObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/DataObjects/ItemMaterials")]
    public class ItemMaterials : ScriptableObject
    {
        [SerializeField] private PlayerColors playerColors;
        
        [Header("Materials")]
        [SerializeField] private Material[] materials;

        private Material runtimeCopy;

        private static readonly int[] PropertyIDs = new int[6];
        
        public Material GetMaterial()
        {
            return runtimeCopy;
        }

        public void SetActivePlayers(PlayerController[] players)
        {
            var playerCount = players.Length;
            runtimeCopy = new Material(materials[playerCount - 1]);

            for (int i = 0; i < playerCount; i++)
            {
                runtimeCopy.SetColor(PropertyIDs[i], playerColors[(int)players[i].Designation]);
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < 6; i++)
            {
                PropertyIDs[i] = Shader.PropertyToID($"_GradientColor{i}");
            }
        }
    }
}