using System.Collections;
using DG.Tweening;
using Meyham.Player;
using Meyham.Set_Up;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.UI
{
    public class ScoreboardView : AGameView, IColoredText, IPlayerNumberDependable
    {
        [Header("Layout")]
        [SerializeField] private float screenWidth;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        
        [Header("Template")]
        [SerializeField] private float templateWidth;
        [SerializeField] private ScoreBoardEntry[] scoreBoardEntries;

        [Header("Parameters")] 
        [SerializeField] private float countUpSpeed;

        private ScoreTweenWrapper[] activeTweens;

        public void SetTextColor(int playerId, Color color)
        {
            scoreBoardEntries[playerId].SetEntryColor(color);
        }

        public void OnPlayerJoined(int playerNumber)
        {
            scoreBoardEntries[playerNumber].gameObject.SetActive(true);
        }

        public void OnPlayerLeft(int playerNumber)
        {
            scoreBoardEntries[playerNumber].gameObject.SetActive(false);
        }

        public IEnumerator CountUpScores(PlayerScore[] playerScores)
        {
            var targetScores = new int[playerScores.Length];
            var winnerScore = 0;
            var winnerIndex = 0;

            for (int i = 0; i < playerScores.Length; i++)
            {
                var score = playerScores[i].GetScore();
                targetScores[i] = score;
                
                if (score < winnerScore)
                {
                    continue;
                }

                winnerIndex = i;
                winnerScore = score;
            }
            
            StartCoroutine(CountUpAnimation(targetScores, winnerIndex, playerScores));
            return new WaitUntil(() => activeTweens[winnerIndex].CurrentScore == winnerScore);
        }

        protected override void Awake()
        {
            foreach (var entry in scoreBoardEntries)
            {
                entry.gameObject.SetActive(false);
            }
            base.Awake();
        }

        private IEnumerator CountUpAnimation(int[] targetScores, int winnerIndex, PlayerScore[] playerScores)
        {
            activeTweens = new ScoreTweenWrapper[targetScores.Length];

            for (int i = 0; i < targetScores.Length; i++)
            {
                activeTweens[i] = new ScoreTweenWrapper(targetScores[i], countUpSpeed);
            }

            while (activeTweens[winnerIndex].IsActive)
            {
                for (var i = 0; i < playerScores.Length; i++)
                {
                    var playerScore = playerScores[i];
                    scoreBoardEntries[(int)playerScore.Designation].SetScore(activeTweens[i].CurrentScore.ToString());
                }

                yield return null;
            }
        }
        
        private class ScoreTweenWrapper
        {
            private readonly Tweener tween;

            public int CurrentScore { get; private set; }

            public bool IsActive => tween.active;
            
            public ScoreTweenWrapper(int desiredScore, float desiredDuration)
            {
                CurrentScore = 0;
                tween = DOTween.To(() => CurrentScore, score => CurrentScore = score, desiredScore, desiredDuration);
            }
        }
    }
}