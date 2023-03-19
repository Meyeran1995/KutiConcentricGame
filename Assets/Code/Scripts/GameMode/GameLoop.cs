using System;
using Meyham.Collision;
using Meyham.Cutscenes;
using Meyham.EditorHelpers;
using Meyham.Set_Up;
using Meyham.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.GameMode
{
    public class GameLoop : MonoBehaviour
    {
        [Header("Steps")]
        [SerializeField] private AGameStep[] steps;
        
        [Header("References")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private PlayerSelectionAnimator playerSelectionAnimator;
        [SerializeField] private PlayerCollisionResolver playerCollisionResolver;

        [Header("Views")]
        [SerializeField] private MainMenuView mainMenuView; 
        [SerializeField] private InGameView inGameView; 
        [SerializeField] private ScoreboardView scoreboardView;
        
        [Header("Debug")] 
        [SerializeField, ReadOnly] private GameSteps currentStep;

        private void Awake()
        {
            Random.InitState(DateTime.Now.Second);
            currentStep = GameSteps.Select;
            
            foreach (var step in steps)
            {
                step.SeTup();
            }
        }

        private void Start()
        {
            foreach (var step in steps)
            {
                step.Link(this);
            }

            var firstStep = steps[0];
            firstStep.StepFinished += OnStepFinished;
            firstStep.Activate();
        }

        public void LinkPlayerManager(Action<PlayerManager> linkAction)
        {
            linkAction.Invoke(playerManager);
        }

        public void LinkPlayerCollisionResolver(Action<PlayerCollisionResolver> linkAction)
        {
            linkAction.Invoke(playerCollisionResolver);
        }
        
        public void LinkMainMenuView(Action<MainMenuView> linkAction)
        {
            linkAction.Invoke(mainMenuView);
        }
        
        public void LinkInGameView(Action<InGameView> linkAction)
        {
            linkAction.Invoke(inGameView);
        }
        
        public void LinkScoreboardView(Action<ScoreboardView> linkAction)
        {
            linkAction.Invoke(scoreboardView);
        }

        public void LinkPlayerSelectionAnimation(Action<PlayerSelectionAnimator> linkAction)
        {
            linkAction.Invoke(playerSelectionAnimator);
        }
        
        private void OnStepFinished()
        {
            steps[(int)currentStep].StepFinished -= OnStepFinished;
            
            currentStep++;

            if ((int)currentStep >= steps.Length)
            {
                currentStep = GameSteps.Start;
            }
            
            var step = steps[(int)currentStep];
            step.StepFinished += OnStepFinished;
            step.Activate();
        }

        private enum GameSteps
        {
            Select,
            Start,
            Match,
            Scoring,
            End,
        }
    }
}
