using System.Collections;
using Meyham.Collision;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.GameMode;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class MatchStep : AGameStep
    {
        [Header("Timer Values")]
        [SerializeField] private FloatValue timeUnit;
        [SerializeField, ReadOnly] private float currentTime;

        private TimerUi timerUi;

        private PlayerManager playerManager;
        
        private PlayerCollisionResolver collisionResolver;

        private WaveManager waveManager;

        private InGameView inGameView;
        
        private float startingTime;

        public override void SeTup()
        {
            startingTime = TimerUi.NumberOfDots * timeUnit;
            
            waveManager = FindAnyObjectByType<WaveManager>(FindObjectsInactive.Include);
            collisionResolver = FindAnyObjectByType<PlayerCollisionResolver>(FindObjectsInactive.Include);

            inGameView = FindAnyObjectByType<InGameView>();
            timerUi = FindAnyObjectByType<TimerUi>();
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
        }

        public override void Deactivate()
        {
            playerManager.DisablePlayers();
            waveManager.enabled = false;
            collisionResolver.enabled = false;
            
            inGameView.CloseView();
            
            base.Deactivate();
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }
        
        private void OnEnable()
        {
            inGameView.OpenView();
            
            currentTime = startingTime;
            StartCoroutine(TimerRoutine());
            
            playerManager.EnablePlayers();
            waveManager.enabled = true;
            collisionResolver.enabled = true;
        }

        private IEnumerator TimerRoutine()
        {
            yield return new WaitForSeconds(timeUnit);

            timerUi.DepleteTime();
            currentTime -= timeUnit;

            if (currentTime <= 0f)
            {                
                Deactivate();
                yield break;
            }

            StartCoroutine(TimerRoutine());
        }
    }
}