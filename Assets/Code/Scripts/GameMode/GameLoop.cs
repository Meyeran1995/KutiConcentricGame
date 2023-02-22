using System;
using Meyham.EditorHelpers;
using Meyham.Set_Up;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Meyham.GameMode
{
    public class GameLoop : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AGameStep[] steps;
        [SerializeField] private PlayerManager playerManager;

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
