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
        [SerializeField] private VoidEventChannelSO onReleasedEvent, endOfSpawningEvent, lastItemVanishedEvent;
        
        [Header("Debug")] 
        [ReadOnly, SerializeField] private int currentWave;
        [ReadOnly, SerializeField] private int spawnCount;
        [ReadOnly, SerializeField] private bool isSpawning, shouldSpawn;

        protected override void OnGameStart()
        {
            shouldSpawn = true;
            SpawnWave();
            onReleasedEvent += OnCollectibleReleased;
            endOfSpawningEvent += OnEndOfSpawns;
        }

        protected override void OnGameEnd()
        {
            onReleasedEvent -= OnCollectibleReleased;
        }

        protected override void OnGameRestart()
        {
            isSpawning = false;
            shouldSpawn = true;
            SpawnWave();
            onReleasedEvent += OnCollectibleReleased;
        }

        private void OnEndOfSpawns()
        {
            shouldSpawn = false;
            StopAllCoroutines();

            if(spawnCount != 0) return;
            
            lastItemVanishedEvent.RaiseEvent();
        }

        private void SpawnWave()
        {
            if(isSpawning || !shouldSpawn) return;
            
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
            spawnCount--;

            if (!shouldSpawn && spawnCount == 0)
            {
                lastItemVanishedEvent.RaiseEvent();
                return;
            }
            
            if(spawnCount != 0) return;
            
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