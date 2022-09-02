using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;

namespace Meyham.GameMode
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private CollectibleWave[] waves;

        [Header("References")] 
        [SerializeField] private CollectibleSpawner spawner;
        
        [Header("Debug")] 
        [ReadOnly, SerializeField] private int currentWave;

        private void Start()
        {
            SpawnWave();
        }

        private void SpawnWave()
        {
            currentWave = ++currentWave % waves.Length;
            foreach (var content in waves[currentWave].Wave)
            {
                spawner.GetCollectible(content);
            }
        }

#if UNITY_EDITOR

        public void EditorSpawnWave()
        {
            SpawnWave();
        }
        
#endif
    }
}