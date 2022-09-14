using System.Collections;
using Meyham.Events;
using Meyham.Set_Up;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.UI
{
    public class ScoreboardView : AGameView
    {
        [Header("References")]
        [SerializeField] private ScoreUI scoreUI;
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO gameStart;
        [Header("Layout")]
        [SerializeField] private float screenWidth;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        [Header("Template")]
        [SerializeField] private GameObject boardTemplate;
        [SerializeField] private float templateWidth;
        
        private void Start()
        {
            gameStart += OnGameStart;
        }

        private void OnGameStart()
        {
            gameStart -= OnGameStart;

            int numberOfPlayers = PlayerManager.NumberOfActivePlayers;

            StartCoroutine(SpawnEntries(numberOfPlayers));
            CenterScoreboardEntries(numberOfPlayers);
        }

        private void CenterScoreboardEntries(int numberOfPlayers)
        {
            float spacedWidth = templateWidth + layoutGroup.spacing;
            float scoreBoardWidth = (numberOfPlayers - 1) * spacedWidth + templateWidth;
            layoutGroup.padding.left = (int)(screenWidth - scoreBoardWidth) / 2;
        }

        private IEnumerator SpawnEntries(int numberOfPlayers)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Instantiate(boardTemplate, transform).SetActive(false);
                yield return null;
            }
        }
        
        public override void OpenView(int animatorId)
        {
            gameObject.SetActive(true);
        }

        public override void CloseView(int animatorId)
        {
            scoreUI.ResetScores();
            gameObject.SetActive(false);
        }
    }
}