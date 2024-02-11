using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.Events;
using UnityEngine;

namespace Meyham.GameMode
{
    public class WaveManager : MonoBehaviour
    {
        private const int max_number_of_spawns = 30;
        
        [Header("Spawning")]
        [SerializeField] private AnimationCurve curveSpawnInterval;
        [Space]
        [SerializeField] private AnimationCurve curveItemDistribution;
        [Space]
        [SerializeField] private AnimationCurve splineStraightProbability;
        [Space]
        [SerializeField] private AnimationCurve splineCurvyProbability;
        
        [Header("Items")]
        [SerializeField] private ItemData[] addBodyPartItems;
        [SerializeField] private ItemData[] takeBodyPartItems;
        
        [Header("References")] 
        [SerializeField] private CollectiblePool pool;
        [SerializeField] private VoidEventChannelSO onReleasedEvent;
        
        [Header("Debug")] 
        [ReadOnly, SerializeField] private float spawnTime;
        [ReadOnly, SerializeField] private float currentTimeInMinutes;
        [ReadOnly, SerializeField] private int spawnCount;

        private void Awake()
        {
            onReleasedEvent += OnCollectibleReleased;
        }

        private void OnEnable()
        {
            currentTimeInMinutes = 0f;
            spawnTime = curveSpawnInterval.Evaluate(0f) / 2f;
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            currentTimeInMinutes += deltaTime / 60;
            spawnTime += deltaTime;
            
            if (spawnCount >= max_number_of_spawns ||
                spawnTime < curveSpawnInterval.Evaluate(currentTimeInMinutes)) return;

            spawnTime = 0f;
            
            SpawnCollectible();
        }
        
        private void SpawnCollectible()
        {
            spawnCount++;
            var collectibleData = SelectCollectible();
            
            pool.GetCollectible(collectibleData);
        }

        private ItemData SelectCollectible()
        {
            var selector = Random.value;
            var probabilityGoodItem = curveItemDistribution.Evaluate(currentTimeInMinutes);

            if (probabilityGoodItem >= selector)
            {
                return addBodyPartItems[GetCurveIndex(selector)];
            }
            
            return takeBodyPartItems[GetCurveIndex(selector)];
        }

        private int GetCurveIndex(float selector)
        {
            var currentProbability = splineStraightProbability.Evaluate(currentTimeInMinutes);

            if (currentProbability >= selector)
            {
                return 0;
            }
            
            currentProbability = splineCurvyProbability.Evaluate(currentTimeInMinutes);

            if (currentProbability >= selector)
            {
                return 1;
            }

            return 2;
        }

        private void OnCollectibleReleased()
        {
            spawnCount--;
        }
    }
}