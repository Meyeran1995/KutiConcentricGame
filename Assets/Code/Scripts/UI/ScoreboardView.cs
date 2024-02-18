using System.Collections.Generic;
using Meyham.Animation;
using Meyham.Events;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.UI
{
    public class ScoreboardView : AGameView, IPlayerNumberDependable
    {
        [Header("References")]
        [SerializeField] private GenericEventChannelSO<int> playerDestroyed;
        [SerializeField] private PlayerColors playerColors;
        [Space]
        [SerializeField] private ScoreBoardEntry[] scoreBoardEntries;
        [Space] 
        [SerializeField] private ScoringParticle[] scoringParticles;

        private readonly List<int> placements = new();

        private int playerCount;

        public void OnPlayerJoined(int playerNumber)
        {
            playerCount++;
        }

        public void OnPlayerLeft(int playerNumber)
        {
            playerCount--;
        }

        public void LockPlayerCount()
        {
            for (var i = 0; i < playerCount; i++)
            {
                var particle = scoringParticles[i];
                particle.SetWorldPosition(scoreBoardEntries[i].transform as RectTransform);
            }
            
            for (int i = playerCount; i < scoreBoardEntries.Length; i++)
            {
                scoreBoardEntries[i].gameObject.SetActive(false);
            }
        }
        
        public void SetUpPlacements()
        {
            for (var i = 0; i < placements.Count; i++)
            {
                var color = playerColors[placements[i]];
                
                scoreBoardEntries[i].SetEntryColor(color);
                scoringParticles[i].SetColor(i, color);
            }
        }

        public override void Clean()
        {
            placements.Clear();
        }
        
        protected override void Awake()
        {
            for (var i = 0; i < scoreBoardEntries.Length; i++)
            {
                var entry = scoreBoardEntries[i];
                entry.SetPlacement(i + 1);
            }

            base.Awake();
        }

        private void Start()
        {
            playerDestroyed += OnPlayerDestroyed;
        }

        private void OnPlayerDestroyed(int designation)
        {
            placements.Add(designation);
        }
    }
}