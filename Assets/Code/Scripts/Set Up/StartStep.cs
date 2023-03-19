using System.Collections;
using Meyham.Collision;
using Meyham.Cutscenes;
using Meyham.GameMode;
using Meyham.Player;
using Meyham.UI;
using UnityEngine;

namespace Meyham.Set_Up
{
    public class StartStep : AGameStep
    {
        [SerializeField] private InGameView inGameView;

        [SerializeField] private PlayerSelectionAnimator playerSelection;

        private PlayerCollisionResolver collisionResolver;

        private PlayerManager playerManager;

        private PlayerStartingPositionProvider startingPositionsProvider;

        private int numberOfPlayers;

        public override void SeTup()
        {
            collisionResolver = FindAnyObjectByType<PlayerCollisionResolver>(FindObjectsInactive.Include);
        }

        public override void Link(GameLoop loop)
        {
            loop.LinkPlayerManager(LinkPlayerManager);
        }

        public override void Activate()
        {
            base.Activate();

            StartCoroutine(WaitForViewToOpen());
        }

        private void LinkPlayerManager(PlayerManager manager)
        {
            playerManager = manager;
        }

        private void SetPlayersToStartingPositions(PlayerController[] players)
        {
            startingPositionsProvider.RotateStartingPositions();
            
            for (int i = 0; i < players.Length; i++)
            {
                players[i].SetStartingPosition(startingPositionsProvider.GetStartingPosition(i));
            }
        }

        private void SetUp()
        {
            var players = playerManager.GetPlayers();

            if (numberOfPlayers == players.Length)
            {
                SetPlayersToStartingPositions(players);
                return;
            }

            numberOfPlayers = players.Length;
            startingPositionsProvider = new PlayerStartingPositionProvider(players.Length);
            
            var collisions = new PlayerCollision[numberOfPlayers];
            var playerIndicesInOrder = playerSelection.PlayerSelectionOrder();

            for (int i = 0; i < players.Length; i++)
            {
                var player = players[playerIndicesInOrder[i]];
                player.SetStartingPosition(startingPositionsProvider.GetStartingPosition(i));
                collisions[i] = player.GetComponentInChildren<PlayerCollision>(true);
            }
            
            collisionResolver.SetPlayerCollisions(collisions);
        }
        
        private IEnumerator WaitForViewToOpen()
        {
            SetUp();

            yield return inGameView.OpenView();
            
            Deactivate();
        }
    }
}