using System.Collections;
using Meyham.Collision;
using Meyham.Cutscenes;
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
        [SerializeField] private GenericEventChannelSO<int> playerDestroyedEventChannel;
        
        [Header("Debug")]
        [SerializeField, ReadOnly] private int alivePlayers;

        private PlayerManager playerManager;
        
        private PlayerCollisionResolver collisionResolver;

        private WaveManager waveManager;

        public override void Setup()
        {
            lastItemVanished += OnLastItemVanished;
            playerDestroyedEventChannel += OnPlayerDestroyed;

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
            collisionResolver.enabled = true;
            waveManager.enabled = true;
            
            playerManager.EnablePlayers();
            alivePlayers = playerManager.PlayerCount;

            setHoldInteractionEventChannel.RaiseEvent(true);
        }

        private void OnPlayerDestroyed(int playerDesignation)
        {
            playerManager.DisablePlayer(playerDesignation);
            alivePlayers--;
            
            if (alivePlayers > 0) return;
            
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