using System.Collections.Generic;
using Meyham.Animation;
using Meyham.Collision;
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
        [SerializeField] private GenericEventChannelSO<bool> setHoldInteractionEventChannel;
        [SerializeField] private GenericEventChannelSO<int> playerDestroyedEventChannel;
        [SerializeField] private GenericEventChannelSO<int> bodyPartCollectedEventChannel;
        
        [Header("Debug")]
        [SerializeField, ReadOnly] private int alivePlayers;

        private PlayerManager playerManager;
        
        private PlayerCollisionResolver collisionResolver;

        private WaveManager waveManager;

        private readonly Dictionary<int, int> bodyPartsCollected = new();

        public override void Setup()
        {
            playerDestroyedEventChannel += OnPlayerDestroyed;
            bodyPartCollectedEventChannel += OnBodyPartCollected;

            waveManager = FindAnyObjectByType<WaveManager>(FindObjectsInactive.Include);
        }

        private void OnBodyPartCollected(int playerDesignation)
        {
            if (bodyPartsCollected.TryGetValue(playerDesignation, out var count))
            {
                bodyPartsCollected[playerDesignation] = count + 1;
                return;
            }
            
            bodyPartsCollected[playerDesignation] = 1;
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

            var activePlayers = playerManager.GetPlayers();
            var playerIDs = new int[activePlayers.Length];
            var playerAngles = new float[activePlayers.Length];
            
            for (int i = 0; i < playerIDs.Length; i++)
            {
                playerIDs[i] = (int)activePlayers[i].Designation;
                playerAngles[i] = activePlayers[i].GetCurrentCirclePosition();
            }
            
            rotatingCutscene.MoveAllPlayersOutsideTheCircleInstant(playerIDs);
            
            base.Deactivate();
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
            
            bodyPartsCollected.Clear();
        }

        private void OnPlayerDestroyed(int playerDesignation)
        {
            playerManager.DisablePlayer(playerDesignation);
            alivePlayers--;
            
            if (alivePlayers > 0) return;
            
            waveManager.enabled = false;
            Deactivate();
        }
    }
}