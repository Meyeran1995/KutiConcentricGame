using System.Collections;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.Events;
using UnityEngine;

namespace Meyham.GameMode
{
    public class WaveManager : AGameLoopSystem
    {
        [Header("Properties")]
        [SerializeField] private float spawnInterval;
        [SerializeField] private CollectibleWave[] waves;

        [Header("References")] 
        [SerializeField] private CollectibleSpawner spawner;
        [SerializeField] private VoidEventChannelSO onReleasedEvent;
        
        [Header("Debug")] 
        [ReadOnly, SerializeField] private int currentWave;
        [ReadOnly, SerializeField] private int spawnCount;

        private bool isSpawning;

        protected override void OnGameStart()
        {
            SpawnWave();
            onReleasedEvent += OnCollectibleReleased;
        }

        protected override void OnGameEnd()
        {
            onReleasedEvent -= OnCollectibleReleased;
        }

        protected override void OnGameRestart()
        {
            SpawnWave();
            onReleasedEvent += OnCollectibleReleased;
        }

        private void SpawnWave()
        {
            if(isSpawning) return;
            
            currentWave = ++currentWave % waves.Length;
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            foreach (var content in waves[currentWave].Wave)
            {
                spawner.GetCollectible(content);
                spawnCount++;
                yield return new WaitForSeconds(spawnInterval);
            }

            isSpawning = false;
        }

        private void OnCollectibleReleased()
        {
            if(--spawnCount != 0) return;
            
            SpawnWave();
        }

#if UNITY_EDITOR

        public void EditorSpawnWave()
        {
            SpawnWave();
        }
        
#endif
    }
}