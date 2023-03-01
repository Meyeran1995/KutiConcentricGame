using System.Collections;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.Events;
using UnityEngine;

namespace Meyham.GameMode
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float spawnInterval;
        [SerializeField] private CollectibleWave[] waves;

        [Header("References")] 
        [SerializeField] private CollectibleSpawner spawner;
        [SerializeField] private VoidEventChannelSO onReleasedEvent, lastItemVanishedEvent;
        
        [Header("Debug")] 
        [ReadOnly, SerializeField] private int currentWave;
        [ReadOnly, SerializeField] private int spawnCount;
        [ReadOnly, SerializeField] private bool isSpawning;

        private void Awake()
        {
            onReleasedEvent += OnCollectibleReleased;
        }

        private void OnEnable()
        {
            SpawnWave();
        }

        private void OnDisable()
        {
            StopAllCoroutines();

            if (spawnCount != 0) return;
            
            lastItemVanishedEvent.RaiseEvent();
        }

        private void SpawnWave()
        {
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
            currentWave = ++currentWave % waves.Length;
        }

        private void OnCollectibleReleased()
        {
            spawnCount--;

            if (spawnCount > 0) return;

            if (!enabled)
            {
                lastItemVanishedEvent.RaiseEvent();
                return;
            }
            
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