using System.Collections;
using DG.Tweening;
using Meyham.Player;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.UI
{
    public class ScoreboardView : AGameView, IColoredText, IPlayerNumberDependable
    {
        [Header("References")]
        [SerializeField] private ScoreBoardEntry[] scoreBoardEntries;

        [Header("Parameters")] 
        [SerializeField] private float maxCountUpTime;
        [SerializeField] private float minCountUpTime;

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

        public WaitForSeconds CountUpScores(PlayerScore[] playerScores)
        {
            var targetScores = new int[playerScores.Length];
            var winnerScore = playerScores[0].GetScore();
            var winnerIndex = 0;
            var loserScore = playerScores[0].GetScore();
            var loserIndex = 0;

            for (int i = 1; i < playerScores.Length; i++)
            {
                var score = playerScores[i].GetScore();
                targetScores[i] = score;
                
                if (score > winnerScore)
                {
                    winnerScore = score;
                    winnerIndex = i;
                    continue;
                }

                if (score >= loserScore) continue;
                
                loserScore = score;
                loserIndex = i;
            }

            StartCoroutine(CountUpAnimation(targetScores, winnerIndex, loserIndex, playerScores));
            return new WaitForSeconds(maxCountUpTime);
        }

        public override void Clean()
        {
            activeTweens = null;

            foreach (var entry in scoreBoardEntries)
            {
                entry.SetScore("0");
            }
        }
        
        protected override void Awake()
        {
            foreach (var entry in scoreBoardEntries)
            {
                entry.gameObject.SetActive(false);
            }
            base.Awake();
        }

        private IEnumerator CountUpAnimation(int[] targetScores, int winnerIndex, int loserIndex, PlayerScore[] playerScores)
        {
            activeTweens = new ScoreTweenWrapper[targetScores.Length];

            for (int i = 0; i < targetScores.Length; i++)
            {
                var score = targetScores[i];
                var countUpInterpolationValue = Mathf.InverseLerp(targetScores[loserIndex], targetScores[winnerIndex], score);
                activeTweens[i] = new ScoreTweenWrapper(score, Mathf.Lerp(minCountUpTime, maxCountUpTime, countUpInterpolationValue));
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