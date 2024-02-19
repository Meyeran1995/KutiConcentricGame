using System.Collections;
using Meyham.GameMode;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class ScoringStep : AGameStep
    {
        [SerializeField] private float endOfGameDelay;

        private ScoreboardView scoreboard;

        private MatchStep matchStep;
        
        public override void Setup()
        {
            matchStep = FindAnyObjectByType<MatchStep>();
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkScoreboardView(LinkView);
        }
        
        private void LinkView(ScoreboardView view)
        {
            scoreboard = view;
        }

        private void OnEnable()
        {
            StartCoroutine(DisplayScoreboard());
        }

        private IEnumerator DisplayScoreboard()
        {
            yield return new WaitForEndOfFrame();
            
            scoreboard.SetUpPlacements(matchStep.GetCollectionStats());
            
            yield return scoreboard.OpenView();

            yield return new WaitForSeconds(endOfGameDelay);
            
            Deactivate();
        }
    }
}