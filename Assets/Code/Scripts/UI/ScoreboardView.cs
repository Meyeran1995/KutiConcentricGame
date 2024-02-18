using System.Collections.Generic;
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

        private readonly List<int> placements = new();

        private int playerCount;

        public void OnPlayerJoined(int playerNumber)
        {
            scoreBoardEntries[playerCount].gameObject.SetActive(true);
            playerCount++;
        }

        public void OnPlayerLeft(int playerNumber)
        {
            playerCount--;
            scoreBoardEntries[playerCount].gameObject.SetActive(false);
        }

        public void SetUpPlacements()
        {
            for (var i = 0; i < placements.Count; i++)
            {
                scoreBoardEntries[i].SetEntryColor(playerColors[placements[i]]);
            }
        }

        public override void Clean()
        {
            placements.Clear();
            //TODO: deactivate particles
        }
        
        protected override void Awake()
        {
            for (var i = 0; i < scoreBoardEntries.Length; i++)
            {
                var entry = scoreBoardEntries[i];
                entry.gameObject.SetActive(false);
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