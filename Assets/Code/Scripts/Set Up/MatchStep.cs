using System.Collections;
using Meyham.Collision;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class MatchStep : AGameStep
    {
        [Header("References")]
        [SerializeField] private VoidEventChannelSO lastItemVanished;
        
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

            inGameView = FindAnyObjectByType<InGameView>(FindObjectsInactive.Include);
            timerUi = FindAnyObjectByType<TimerUi>(FindObjectsInactive.Include);
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
            lastItemVanished += OnLastItemVanished;
        }

        public override void Deactivate()
        {
            playerManager.DisablePlayers();
            collisionResolver.enabled = false;

            StartCoroutine(WaitForViewToClose());
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }
        
        private void OnEnable()
        {
            currentTime = startingTime;
            StartCoroutine(TimerRoutine());
            
            collisionResolver.enabled = true;
            waveManager.enabled = true;
            
            playerManager.EnablePlayers();
        }

        private IEnumerator TimerRoutine()
        {
            while (currentTime > 0f)
            {
                yield return new WaitForSeconds(timeUnit);

                timerUi.DepleteTime();
                currentTime -= timeUnit;
            }
            
            OnTimerElapsed();
        }

        private void OnTimerElapsed()
        {
            timerUi.gameObject.SetActive(false);
            waveManager.enabled = false;
        }
        
        private void OnLastItemVanished()
        {
            Deactivate();
        }

        private IEnumerator WaitForViewToClose()
        {
            yield return inGameView.CloseView();
            
            base.Deactivate();
        }
    }
}