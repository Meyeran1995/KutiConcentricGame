using System.Collections;
using Meyham.Collision;
using Meyham.Cutscenes;
using Meyham.DataObjects;
using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class MatchStep : AGameStep
    {
        [SerializeField] private RotatingCutscene rotatingCutscene;
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO lastItemVanished;
        [SerializeField] private GenericEventChannelSO<bool> setHoldInteractionEventChannel;
        
        [Header("Timer Values")]
        [SerializeField] private FloatParameter startingTime;
        [SerializeField, ReadOnly] private float currentTime;

        private PlayerManager playerManager;
        
        private PlayerCollisionResolver collisionResolver;

        private WaveManager waveManager;

        public override void Setup()
        {
            lastItemVanished += OnLastItemVanished;

            waveManager = FindAnyObjectByType<WaveManager>(FindObjectsInactive.Include);
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
            loop.LinkPlayerCollisionResolver(LinkCollisionResolver);
        }

        public override void Deactivate()
        {
            setHoldInteractionEventChannel.RaiseEvent(false);
            
            playerManager.DisablePlayers();
            collisionResolver.enabled = false;

            StartCoroutine(WaitForViewToClose());
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }
        
        private void LinkCollisionResolver(PlayerCollisionResolver resolver)
        {
            collisionResolver = resolver;
        }
        
        private void OnEnable()
        {
            currentTime = startingTime;
            StartCoroutine(TimerRoutine());
            
            collisionResolver.enabled = true;
            waveManager.enabled = true;
            
            playerManager.EnablePlayers();
            setHoldInteractionEventChannel.RaiseEvent(true);
        }

        private IEnumerator TimerRoutine()
        {
            while (currentTime > 0f)
            {
                yield return new WaitForSeconds(startingTime);

                currentTime -= startingTime;
            }
            
            OnTimerElapsed();
        }

        private void OnTimerElapsed()
        {
            waveManager.enabled = false;
        }
        
        private void OnLastItemVanished()
        {
            Deactivate();
        }

        private IEnumerator WaitForViewToClose()
        {
            var activePlayers = playerManager.GetPlayers();
            var playerIDs = new int[activePlayers.Length];
            var playerAngles = new float[activePlayers.Length];

            for (int i = 0; i < playerIDs.Length; i++)
            {
                playerIDs[i] = (int)activePlayers[i].Designation;
                playerAngles[i] = activePlayers[i].GetCurrentCirclePosition();
            }
            
            rotatingCutscene.MoveAllPlayersIntoTheCircle(playerIDs);
            rotatingCutscene.UpdateCirclePositions(playerIDs, playerAngles);
            rotatingCutscene.gameObject.SetActive(true);
            playerManager.HidePlayers();
            
            yield return rotatingCutscene.AnimateAllPlayersLeavingTheCircle(playerIDs);
            
            base.Deactivate();
        }
    }
}